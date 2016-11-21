using System.Web.Mvc;
using EPiServer.Reference.Commerce.Site.B2B.Filters;
using EPiServer.Reference.Commerce.Site.Features.OrderPads.Pages;
using EPiServer.Reference.Commerce.Site.Features.OrderPads.ViewModels;
using EPiServer.Web.Mvc;

namespace EPiServer.Reference.Commerce.Site.Features.OrderPads.Controllers
{
    [Authorize]
    public class OrderPadsPageController : PageController<OrderPadsPage>
    {
        [NavigationAuthorize("Admin,Approver")]
        public ActionResult Index(OrderPadsPage currentPage)
        {
            var viewModel = new OrderPadsPageViewModel { CurrentPage = currentPage };
            return View(viewModel);
        }
    }
}