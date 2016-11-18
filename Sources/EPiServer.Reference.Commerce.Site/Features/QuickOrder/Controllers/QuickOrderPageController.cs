using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Castle.Core.Internal;
using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.Reference.Commerce.Site.Features.Cart.Services;
using EPiServer.Reference.Commerce.Site.Features.QuickOrder.Pages;
using EPiServer.Reference.Commerce.Site.Features.QuickOrder.ViewModels;
using EPiServer.Reference.Commerce.Site.Infrastructure.Attributes;
using EPiServer.Web.Mvc;
using Mediachase.Commerce.Catalog;

namespace EPiServer.Reference.Commerce.Site.Features.QuickOrder.Controllers
{
    [Authorize]
    public class QuickOrderPageController : PageController<QuickOrderPage>
    {
        private readonly IQuickOrderService _quickOrderService;
        private readonly ICartService _cartService;
        private ICart _cart;
        private readonly IOrderRepository _orderRepository;
        private readonly ReferenceConverter _referenceConverter;

        public QuickOrderPageController(
            IQuickOrderService quickOrderService,
            ICartService cartService,
            IOrderRepository orderRepository,
            ReferenceConverter referenceConverter)
        {
            _quickOrderService = quickOrderService;
            _cartService = cartService;
            _orderRepository = orderRepository;
            _referenceConverter = referenceConverter;
        }

        public ActionResult Index(QuickOrderPage currentPage)
        {
            var messages = TempData["messages"] as List<string>;
            var viewModel = new QuickOrderPageViewModel
            {
                CurrentPage = currentPage,
                ReturnedMessages = messages
            };
            return View(viewModel);
        }

        [HttpPost]
        [AllowDBWrite]
        public ActionResult Import(QuickOrderPageViewModel viewModel)
        {
            string warningMessage = string.Empty;
            var returnedMessages = new List<string>();

            ModelState.Clear();

            if (Cart == null)
            {
                _cart = _cartService.LoadOrCreateCart(_cartService.DefaultCartName);
            }
            
            foreach (var product in viewModel.ProductsList)
            {
                ContentReference variationReference = _referenceConverter.GetContentLink(product.Sku);
                var responseMessage = _quickOrderService.ValidateProduct(variationReference, Convert.ToDecimal(product.Quantity), product.Sku);
                if (responseMessage.IsNullOrEmpty())
                {
                    if (_cartService.AddToCart(Cart, product.Sku, out warningMessage))
                    {
                        _cartService.ChangeCartItem(Cart, 0, product.Sku, product.Quantity, "", "");
                        _orderRepository.Save(Cart);
                    }
                }
                else
                {
                    returnedMessages.Add(responseMessage);
                }
            }
            if (returnedMessages.Count == 0)
            {
                returnedMessages.Add("All items were added to cart.");
            }
            TempData["messages"] = returnedMessages;
            return RedirectToAction("Index", new { messages = returnedMessages });
        }

        public JsonResult GetSku(string query)
        {
            var data = new[] {
              new { sku = "1111", productName = "product1", unitPrice = "5"},
              new { sku = "222", productName = "product2", unitPrice = "15"},
              new { sku = "312", productName = "product3", unitPrice = "25"}
           };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        private ICart Cart
        {
            get { return _cart ?? (_cart = _cartService.LoadCart(_cartService.DefaultCartName)); }
        }
    }
}