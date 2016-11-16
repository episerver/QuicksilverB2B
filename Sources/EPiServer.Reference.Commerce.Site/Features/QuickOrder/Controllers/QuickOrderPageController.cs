using System.Web.Mvc;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.Reference.Commerce.Site.Features.QuickOrder.Pages;
using EPiServer.Reference.Commerce.Site.Features.QuickOrder.ViewModels;
using EPiServer.Reference.Commerce.Site.Infrastructure.Attributes;
using EPiServer.Web.Mvc;

namespace EPiServer.Reference.Commerce.Site.Features.QuickOrder.Controllers
{ 
    [Authorize]
    public class QuickOrderPageController : PageController<QuickOrderPage>
    {
        private readonly IQuickOrderService _quickOrderService;

        public QuickOrderPageController(IQuickOrderService quickOrderService)
        {
            _quickOrderService = quickOrderService;
        }

        public ActionResult Index(QuickOrderPage currentPage)
        {
            var viewModel = new QuickOrderPageViewModel { CurrentPage = currentPage };
            return View(viewModel);
        }

        [HttpPost]
        [AllowDBWrite]
        public ActionResult Import(QuickOrderPageViewModel viewModel)
        {
            return RedirectToAction("Index");
        }
    }
}