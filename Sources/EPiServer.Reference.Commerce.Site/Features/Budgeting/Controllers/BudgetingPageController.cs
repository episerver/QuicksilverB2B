using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer.Reference.Commerce.Site.Features.Budgeting.Pages;
using EPiServer.Reference.Commerce.Site.Features.Budgeting.ViewModels;
using EPiServer.Web.Mvc;

namespace EPiServer.Reference.Commerce.Site.Features.Budgeting.Controllers
{
    [Authorize]
    public class BudgetingPageController : PageController<BudgetingPage>
    {
        public ActionResult Index(BudgetingPage currentPage)
        {
            var viewModel = new BudgetingPageViewModel { CurrentPage = currentPage };
            return View(viewModel);
        }
        public ActionResult AddBudget(BudgetingPage currentPage)
        {
            var viewModel = new BudgetingPageViewModel { CurrentPage = currentPage };
            return View(viewModel);
        }
        public ActionResult EditBudget(BudgetingPage currentPage)
        {
            var viewModel = new BudgetingPageViewModel { CurrentPage = currentPage };
            return View(viewModel);
        }
    }
}