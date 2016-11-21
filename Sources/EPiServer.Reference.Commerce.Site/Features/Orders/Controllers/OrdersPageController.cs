using System.Web.Mvc;
using EPiServer.Reference.Commerce.Site.Features.Orders.Pages;
using EPiServer.Reference.Commerce.Site.Features.Orders.ViewModels;
using EPiServer.Web.Mvc;

namespace EPiServer.Reference.Commerce.Site.Features.Orders.Controllers
{ 
    [Authorize]
    public class OrdersPageController : PageController<OrdersPage>
    {
        public ActionResult Index(OrdersPage currentPage)
        {
            var viewModel = new OrdersPageViewModel { CurrentPage = currentPage };
            return View(viewModel);
        }
    }
}