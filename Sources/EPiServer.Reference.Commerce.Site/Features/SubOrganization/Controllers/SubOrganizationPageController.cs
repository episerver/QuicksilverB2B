using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer.Reference.Commerce.Site.Features.Suborganization.Pages;
using EPiServer.Reference.Commerce.Site.Features.Suborganization.ViewModels;
using EPiServer.Web.Mvc;

namespace EPiServer.Reference.Commerce.Site.Features.Suborganization.Controllers
{
    [Authorize]
    public class SubOrganizationPageController : PageController<SubOrganizationPage>
    {
        public ActionResult Index(SubOrganizationPage currentPage)
        {
            var viewModel = new SubOrganizationPageViewModel { CurrentPage = currentPage };
            return View(viewModel);
        }
        public ActionResult Edit(SubOrganizationPage currentPage)
        {
            var viewModel = new SubOrganizationPageViewModel { CurrentPage = currentPage };
            return View(viewModel);
        }
    }
}