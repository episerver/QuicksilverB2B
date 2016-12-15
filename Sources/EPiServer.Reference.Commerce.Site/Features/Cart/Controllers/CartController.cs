using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Order;
using EPiServer.Reference.Commerce.Site.Features.Cart.Services;
using EPiServer.Reference.Commerce.Site.Features.Cart.ViewModelFactories;
using EPiServer.Reference.Commerce.Site.Infrastructure.Attributes;
using System.Web.Mvc;
using Castle.Core.Internal;
using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.B2B.Enums;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.Reference.Commerce.Site.Features.Start.Pages;
using Mediachase.Commerce.Catalog;
using Constants = EPiServer.Reference.Commerce.Site.B2B.Constants;

namespace EPiServer.Reference.Commerce.Site.Features.Cart.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private ICart _cart;
        private readonly IOrderRepository _orderRepository;
        readonly CartViewModelFactory _cartViewModelFactory;
        private readonly ICartServiceB2B _cartServiceB2B;
        private readonly IQuickOrderService _quickOrderService;
        private readonly ReferenceConverter _referenceConverter;
        private readonly ICustomerService _customerService;
        private readonly IContentLoader _contentLoader;

        public CartController(
            ICartService cartService,
            IOrderRepository orderRepository,
            CartViewModelFactory cartViewModelFactory,
            ICartServiceB2B cartServiceB2B,
            IQuickOrderService quickOrderService,
            ReferenceConverter referenceConverter,
            ICustomerService customerService,
            IContentLoader contentLoader)
        {
            _cartService = cartService;
            _orderRepository = orderRepository;
            _cartViewModelFactory = cartViewModelFactory;
            _cartServiceB2B = cartServiceB2B;
            _quickOrderService = quickOrderService;
            _referenceConverter = referenceConverter;
            _customerService = customerService;
            _contentLoader = contentLoader;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult MiniCartDetails()
        {
            var viewModel = _cartViewModelFactory.CreateMiniCartViewModel(Cart);
            return PartialView("_MiniCartDetails", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult LargeCart()
        {
            var viewModel = _cartViewModelFactory.CreateLargeCartViewModel(Cart);
            return PartialView("LargeCart", viewModel);
        }

        [HttpPost]
        [AllowDBWrite]
        public ActionResult AddToCart(string code)
        {
            string warningMessage = string.Empty;

            ModelState.Clear();

            if (Cart == null)
            {
                _cart = _cartService.LoadOrCreateCart(_cartService.DefaultCartName);
            }

            // If order comes from an quoted order.
            if (Cart.Properties[Constants.Quote.ParentOrderGroupId] != null)
            {
                int orderLink = int.Parse(Cart.Properties[Constants.Quote.ParentOrderGroupId].ToString());
                if (orderLink != 0)
                {
                    return new HttpStatusCodeResult(500, "Invalid operation on quoted cart.");
                }
            }

            if (_cartService.AddToCart(Cart, code, out warningMessage))
            {
                _orderRepository.Save(Cart);
                return MiniCartDetails();
            }

            // HttpStatusMessage can't be longer than 512 characters.
            warningMessage = warningMessage.Length < 512 ? warningMessage : warningMessage.Substring(512);

            return new HttpStatusCodeResult(500, warningMessage);
        }

        [HttpPost]
        [AllowDBWrite]
        public JsonResult AddVariantsToCart(List<string> variants)
        {
            var returnedMessages = new List<string>();

            ModelState.Clear();

            if (Cart == null)
            {
                _cart = _cartService.LoadOrCreateCart(_cartService.DefaultCartName);
            }

            foreach (var product in variants)
            {
                var sku = product.Split(';')[0];
                var quantity = Convert.ToInt32(product.Split(';')[1]);
                
                ContentReference variationReference = _referenceConverter.GetContentLink(sku);

                var responseMessage = _quickOrderService.ValidateProduct(variationReference, Convert.ToDecimal(quantity), sku);
                    if (responseMessage.IsNullOrEmpty())
                    {
                        string warningMessage;
                        if (_cartService.AddToCart(Cart, sku, out warningMessage))
                        {
                            _cartService.ChangeCartItem(Cart, 0, sku, quantity, "", "");
                            _orderRepository.Save(Cart);
                        }
                    }
                    else
                    {
                        returnedMessages.Add(responseMessage);
                    }
            }
            Session[Constants.ErrorMesages] = returnedMessages;

            return Json(returnedMessages, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowDBWrite]
        public JsonResult ClearQuotedCart()
        {
            _cartServiceB2B.DeleteCart(Cart);
            _cart = _cartServiceB2B.CreateNewCart();

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowDBWrite]
        public ActionResult ChangeCartItem(int shipmentId, string code, decimal quantity, string size, string newSize)
        {
            ModelState.Clear();
            if (quantity != 0)
            {
                // If order comes from an quoted order.
                if (Cart.Properties[Constants.Quote.ParentOrderGroupId] != null)
                {
                    int orderLink = int.Parse(Cart.Properties[Constants.Quote.ParentOrderGroupId].ToString());
                    if (orderLink != 0)
                    {
                        return new HttpStatusCodeResult(500, "Invalid operation on quoted cart.");
                    }
                }
            }
          
            _cartService.ChangeCartItem(Cart, shipmentId, code, quantity, size, newSize);
            if (!Cart.GetAllLineItems().Any() && Cart.Properties[Constants.Quote.ParentOrderGroupId] != null)
            {
                _cartServiceB2B.DeleteCart(Cart);
                _cart = _cartServiceB2B.CreateNewCart();
            }
            _orderRepository.Save(Cart);

            return MiniCartDetails();
        }

        [HttpPost]
        [AllowDBWrite]
        public ActionResult RequestQuote()
        {
            bool succesRequest;

            var currentCustomer = _customerService.GetCurrentContact();
            if (currentCustomer.Role != B2BUserRoles.Purchaser)
                return Json(new { result = false });

            if (Cart == null)
            {
                _cart = _cartService.LoadOrCreateCart(_cartService.DefaultCartName);
                succesRequest = _cartServiceB2B.PlaceCartForQuote(_cart);
            }
            else
            {
                succesRequest = _cartServiceB2B.PlaceCartForQuote(Cart);
            }
            _cartServiceB2B.DeleteCart(_cart);
            _cart = _cartServiceB2B.CreateNewCart();

            return Json(new { result = succesRequest });
        }

        [HttpPost]
        [AllowDBWrite]
        public ActionResult RequestQuoteById(int orderId)
        {
            var currentCustomer = _customerService.GetCurrentContact();
            if (currentCustomer.Role != B2BUserRoles.Purchaser)
                return Json(new { result = false });

             var placedOrderId =_cartServiceB2B.PlaceCartForQuoteById(orderId, currentCustomer.ContactId);

            var startPage = _contentLoader.Get<StartPage>(ContentReference.StartPage);

            return RedirectToAction("Index", "OrderDetails",
                new {currentPage = startPage.OrderDetailsPage, orderGroupId = placedOrderId });
        }

        private ICart Cart
        {
            get { return _cart ?? (_cart = _cartService.LoadCart(_cartService.DefaultCartName)); }
        }

        public CartViewModelFactory CartViewModelFactory
        {
            get
            {
                return _cartViewModelFactory;
            }
        }
    }
}