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
using EPiServer.Reference.Commerce.Site.B2B.Filters;
using EPiServer.Reference.Commerce.Site.B2B.Models.Pages;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.Reference.Commerce.Site.Features.Start.Pages;
using EPiServer.Reference.Commerce.Site.Features.Users.ViewModels;
using EPiServer.Reference.Commerce.Site.Infrastructure.Attributes;
using EPiServer.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace EPiServer.Reference.Commerce.Site.Features.Users.Controllers
{
    [Authorize]
    public class UsersPageController : PageController<UsersPage>
    {
        private readonly ICustomerService _customerService;
        private readonly IOrganizationService _organizationService;
        private readonly IContentLoader _contentLoader;
        private readonly IMailService _mailService;
        private readonly ApplicationUserManager _userManager;
        private readonly LocalizationService _localizationService;

        public UsersPageController(ICustomerService customerService, IOrganizationService organizationService, ApplicationUserManager applicationUserManager, IContentLoader contentLoader, IMailService mailService, LocalizationService localizationService)
        {
            _customerService = customerService;
            _organizationService = organizationService;
            _userManager = applicationUserManager;
            _contentLoader = contentLoader;
            _mailService = mailService;
            _localizationService = localizationService;
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
        public ActionResult EditUser(UsersPage currentPage,string id)
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
                Organizations = organization.SubOrganizations ?? new List<OrganizationModel>()
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
            ApplicationUser user = _userManager.FindByEmail(viewModel.Contact.Email);
            if (user != null)
            {
                viewModel.Contact.ExistingUser = true;

                var organization = _organizationService.GetCurrentUserOrganization();
                viewModel.Organizations = organization.SubOrganizations ?? new List<OrganizationModel>();

                return View(viewModel);
            }
            
            SaveUser(viewModel);

            return RedirectToAction("Index");
        }

        [NavigationAuthorize("Admin")]
        public JsonResult GetUsers(string phrase)
        {

            var data = new[] {
              new { fullname= "admin", firstname = "admin", lastname = "admin", email = "admin@yahoo.ro" },
              new { fullname= "Marius Lazar", firstname = "Marius", lastname = "Lazar", email = "mariuslazar@yahoo.ro" },
              new { fullname= "Alex Tiponut", firstname = "Alex", lastname = "Tiponut", email = "alex@yahoo.ro" },
              new { fullname= "Simona Danciu", firstname = "Simona", lastname = "Danciu", email = "simona@yahoo.ro" },
              new { fullname= "Ionut Iancau", firstname = "Ionut", lastname = "Iancau", email = "ionut@yahoo.ro" },
              new { fullname= "Mihai Runcan", firstname = "Mihai", lastname = "Runcan", email = "mihai@yahoo.ro" }
           };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #region Helpers
        private void SaveUser(UsersPageViewModel viewModel)
        {
            var contactUser = new ApplicationUser
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