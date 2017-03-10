using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Internal;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using EPiServer.Globalization;
using EPiServer.Reference.Commerce.Shared.Models;
using EPiServer.Reference.Commerce.Shared.Models.Identity;
using EPiServer.Reference.Commerce.Shared.Services;
using EPiServer.Reference.Commerce.Site.B2B.Enums;
using EPiServer.Reference.Commerce.Site.B2B.Filters;
using EPiServer.Reference.Commerce.Site.B2B.Models.Pages;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.Reference.Commerce.Site.B2B.Services;
using EPiServer.Reference.Commerce.Site.Features.Search.Services;
using EPiServer.Reference.Commerce.Site.Features.Start.Pages;
using EPiServer.Reference.Commerce.Site.Features.Users.ViewModels;
using EPiServer.Reference.Commerce.Site.Infrastructure.Attributes;
using EPiServer.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Reference.Commerce.Shared.Identity;

namespace EPiServer.Reference.Commerce.Site.Features.Users.Controllers
{
    [Authorize]
    public class UsersPageController : PageController<UsersPage>
    {
        private readonly ICustomerService _customerService;
        private readonly IOrganizationService _organizationService;
        private readonly IContentLoader _contentLoader;
        private readonly IMailService _mailService;
        private readonly ApplicationUserManager<SiteUser> _userManager;
        private readonly ApplicationSignInManager<SiteUser> _signInManager;
        private readonly LocalizationService _localizationService;
        private readonly IEPiFindSearchService _ePiFindSearchService;
        private readonly CookieService _cookieService;

        public UsersPageController(ICustomerService customerService, IOrganizationService organizationService,
            ApplicationUserManager<SiteUser> userManager,
            ApplicationSignInManager<SiteUser> signinManager,
            IContentLoader contentLoader, IMailService mailService, LocalizationService localizationService, 
            IEPiFindSearchService ePiFindSearchService, CookieService cookieService)
        {
            _customerService = customerService;
            _organizationService = organizationService;
            _userManager = userManager;
            _signInManager = signinManager;
            _contentLoader = contentLoader;
            _mailService = mailService;
            _localizationService = localizationService;
            _ePiFindSearchService = ePiFindSearchService;
            _cookieService = cookieService;
        }

        [NavigationAuthorize("Admin")]
        public ActionResult Index(UsersPage currentPage)
        {
            var organization = _organizationService.GetCurrentUserOrganization();
            var viewModel = new UsersPageViewModel
            {
                CurrentPage = currentPage,
                Users = _customerService.GetContactsForCurrentOrganization(),
                Organizations = organization.SubOrganizations ?? new List<OrganizationModel>()
            };
            return View(viewModel);
        }

        [NavigationAuthorize("Admin")]
        public ActionResult AddUser(UsersPage currentPage)
        {
            var organization = _organizationService.GetCurrentUserOrganization();
            var viewModel = new UsersPageViewModel
            {
                CurrentPage = currentPage,
                Contact = new ContactViewModel(),
                Organizations = organization.SubOrganizations?? new List<OrganizationModel>()
            };
            return View(viewModel);
        }

        [NavigationAuthorize("Admin")]
        public ActionResult EditUser(UsersPage currentPage, string id)
        {
            if (id.IsNullOrEmpty())
            {
                return RedirectToAction("Index");
            }
            var organization = _organizationService.GetCurrentUserOrganization();
            var contact = _customerService.GetContactById(id);

            var viewModel = new UsersPageViewModel
            {
                CurrentPage = currentPage,
                Contact = contact,
                Organizations = organization.SubOrganizations ?? new List<OrganizationModel>(),
                SubOrganization =
                    contact.Role != B2BUserRoles.Admin
                        ? _organizationService.GetSubOrganizationById(contact.Organization?.OrganizationId.ToString())
                        : new SubOrganizationModel()
            };
            return View(viewModel);
        }

        [NavigationAuthorize("Admin")]
        public ActionResult RemoveUser(string id)
        {
            if (id.IsNullOrEmpty())
            {
                return RedirectToAction("Index");
            }
            _customerService.RemoveContact(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [AllowDBWrite]
        [NavigationAuthorize("Admin")]
        public ActionResult UpdateUser(UsersPageViewModel viewModel)
        {
            _customerService.EditContact(viewModel.Contact);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [AllowDBWrite]
        [NavigationAuthorize("Admin")]
        public ActionResult AddUser(UsersPageViewModel viewModel)
        {
            var user = _userManager.FindByEmail(viewModel.Contact.Email);
            if (user != null)
            {
                if (_customerService.HasOrganization(user.Id))
                {
                    viewModel.Contact.ShowOrganizationError = true;
                    var organization = _organizationService.GetCurrentUserOrganization();
                    viewModel.Organizations = organization.SubOrganizations ?? new List<OrganizationModel>();
                    return View(viewModel);
                }

                _customerService.AddContactToOrganization(user.Id);
                _customerService.UpdateContact(user.Id, viewModel.Contact.UserRole, viewModel.Contact.Location);
            }
            else
                SaveUser(viewModel);

            return RedirectToAction("Index");
        }

        [NavigationAuthorize("Admin")]
        public JsonResult GetUsers(string query)
        {
            var data = _ePiFindSearchService.SearchUsers(query);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAddresses(string id)
        {
            var organization = _organizationService.GetSubOrganizationById(id);
            var addresses = organization.Locations;

            return Json(addresses, JsonRequestBehavior.AllowGet);
        }

        [NavigationAuthorize("Admin")]
        public JsonResult ImpersonateUser(string username)
        {
            var success = false;
            var user = _userManager.FindByEmail(username);
            if (user != null)
            {
                _cookieService.Set(B2B.Constants.Cookies.B2BImpersonatingAdmin, User.Identity.GetUserName(), true);
                _signInManager.SignIn(user, false, false);
                success = true;
            }
            return Json(new {success});
        }

        public ActionResult BackAsAdmin()
        {
            var adminUsername = _cookieService.Get(B2B.Constants.Cookies.B2BImpersonatingAdmin);
            if (!string.IsNullOrEmpty(adminUsername))
            {
                var adminUser = _userManager.FindByEmail(adminUsername);
                if (adminUser != null)
                    _signInManager.SignIn(adminUser, false, false);

                _cookieService.Remove(B2B.Constants.Cookies.B2BImpersonatingAdmin);
            }
            return Redirect(Request.UrlReferrer?.AbsoluteUri ?? "/");
        }

        #region Helpers
        private void SaveUser(UsersPageViewModel viewModel)
        {
            var contactUser = new SiteUser
            {
                UserName = viewModel.Contact.Email,
                Email = viewModel.Contact.Email,
                Password = "password",
                FirstName = viewModel.Contact.FirstName,
                LastName = viewModel.Contact.LastName,
                RegistrationSource = "Registration page"
            };
            IdentityResult result = _userManager.Create(contactUser);

            _customerService.CreateUser(viewModel.Contact, contactUser.Id);

            var user = _userManager.FindByName(viewModel.Contact.Email);
            if (user != null)
            {
                var startPage = _contentLoader.Get<StartPage>(ContentReference.StartPage);
                var body = _mailService.GetHtmlBodyForMail(startPage.ResetPasswordMail, new NameValueCollection(), ContentLanguage.PreferredCulture.TwoLetterISOLanguageName);
                var mailPage = _contentLoader.Get<MailBasePage>(startPage.ResetPasswordMail);
                var code = _userManager.GeneratePasswordResetToken(user.Id);
                var url = Url.Action("ResetPassword", "ResetPassword", new { userId = user.Id, code = HttpUtility.UrlEncode(code), language = ContentLanguage.PreferredCulture.TwoLetterISOLanguageName }, Request.Url.Scheme);

                body = body.Replace("[MailUrl]",
                    string.Format("{0}<a href=\"{1}\">{2}</a>",
                        _localizationService.GetString("/ResetPassword/Mail/Text"),
                        url,
                        _localizationService.GetString("/ResetPassword/Mail/Link")));

                _userManager.SendEmail(user.Id, mailPage.MailTitle, body);
            }
        }
        #endregion
    }
}