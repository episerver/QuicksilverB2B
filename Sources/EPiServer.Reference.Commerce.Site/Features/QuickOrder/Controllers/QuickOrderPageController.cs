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

        public JsonResult GetSku(string phrase)
        {

            var data = new[] {
              new { sku = "1111", productName = "product1", unitPrice = "5"},
              new { sku = "222", productName = "product2", unitPrice = "15"},
              new { sku = "312", productName = "product3", unitPrice = "25"}
           };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}