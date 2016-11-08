using System.Web.Mvc;
using EPiServer.Reference.Commerce.Site.B2B.Models.Pages;
using EPiServer.Reference.Commerce.Site.Features.Organization.ViewModels;
using EPiServer.Web.Mvc;

namespace EPiServer.Reference.Commerce.Site.Features.Organization.Controllers
{
    [Authorize]
    public class OrganizationPageController : PageController<OrganizationPage>
    {
        public ActionResult Index(OrganizationPage currentPage)
        {

            if (Request.QueryString["showForm"] != null && bool.Parse(Request.QueryString["showForm"]))
            {
                return RedirectToAction("Edit");
            }

            var viewModel = new OrganizationPageViewModel { CurrentPage = currentPage };
            return View(viewModel);
        }
        public ActionResult Edit(OrganizationPage currentPage)
        {
            var viewModel = new OrganizationPageViewModel { CurrentPage = currentPage };
            return View(viewModel);
        }
        public ActionResult AddSub(OrganizationPage currentPage)
        {
            var viewModel = new OrganizationPageViewModel { CurrentPage = currentPage };
            return View(viewModel);
        }
    }
}