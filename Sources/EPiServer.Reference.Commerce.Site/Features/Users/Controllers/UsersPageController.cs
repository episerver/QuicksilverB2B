using System.Collections.Generic;
using System.Web.Mvc;
using EPiServer.Reference.Commerce.Site.B2B.Models.Pages;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.Reference.Commerce.Site.Features.Users.ViewModels;
using EPiServer.Reference.Commerce.Site.Infrastructure.Attributes;
using EPiServer.Web.Mvc;

namespace EPiServer.Reference.Commerce.Site.Features.Users.Controllers
{
    [Authorize]
    public class UsersPageController : PageController<UsersPage>
    {
        private readonly ICustomerService _customerService;
        private readonly IOrganizationService _organizationService;

        public UsersPageController(ICustomerService customerService, IOrganizationService organizationService)
        {
            _customerService = customerService;
            _organizationService = organizationService;
        }

        public ActionResult Index(UsersPage currentPage)
        {
            var viewModel = new UsersPageViewModel
            {
                CurrentPage = currentPage,
                Users = _customerService.GetContactsForCurrentOrganization()
            };
            return View(viewModel);
        }
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
        public ActionResult EditUser(UsersPage currentPage)
        {
            var viewModel = new UsersPageViewModel { CurrentPage = currentPage };
            return View(viewModel);
        }

        [HttpPost]
        [AllowDBWrite]
        public ActionResult Save(UsersPageViewModel viewModel)
        {
            _customerService.CreateUser(viewModel.Contact);

            return RedirectToAction("Index");
        }

        public JsonResult GetUsers(string phrase)
        {

            var data = new[] {
              new { name= "marius", email = "mariuslazar@yahoo.ro" },
              new { name= "alex", email = "alex@yahoo.ro" },
              new { name= "simona", email = "simona@yahoo.ro" },
              new { name= "ionut", email = "ionut@yahoo.ro" },
              new { name= "mihai", email = "mihai@yahoo.ro" }
           };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}