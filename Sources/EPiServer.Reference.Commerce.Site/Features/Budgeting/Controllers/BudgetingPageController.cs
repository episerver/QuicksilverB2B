using System.Web.Mvc;
using EPiServer.Reference.Commerce.Site.B2B.Filters;
using EPiServer.Reference.Commerce.Site.Features.Budgeting.Pages;
using EPiServer.Reference.Commerce.Site.Features.Budgeting.ViewModels;
using EPiServer.Web.Mvc;

namespace EPiServer.Reference.Commerce.Site.Features.Budgeting.Controllers
{
    [Authorize]
    public class BudgetingPageController : PageController<BudgetingPage>
    {
        [NavigationAuthorize("Admin,Approver")]
        public ActionResult Index(BudgetingPage currentPage)
        {
            var viewModel = new BudgetingPageViewModel { CurrentPage = currentPage };
            return View(viewModel);
        }

        [NavigationAuthorize("Admin,Approver")]
        public ActionResult AddBudget(BudgetingPage currentPage)
        {
            var viewModel = new BudgetingPageViewModel { CurrentPage = currentPage };
            return View(viewModel);
        }

        [NavigationAuthorize("Admin,Approver")]
        public ActionResult EditBudget(BudgetingPage currentPage)
        {
            var viewModel = new BudgetingPageViewModel { CurrentPage = currentPage };
            return View(viewModel);
        }
    }
}