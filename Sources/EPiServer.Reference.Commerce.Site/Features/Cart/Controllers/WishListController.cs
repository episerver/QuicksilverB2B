using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.Features.Cart.Pages;
using EPiServer.Reference.Commerce.Site.Features.Cart.Services;
using EPiServer.Reference.Commerce.Site.Features.Cart.ViewModelFactories;
using EPiServer.Reference.Commerce.Site.Features.Start.Pages;
using EPiServer.Reference.Commerce.Site.Infrastructure.Attributes;
using EPiServer.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Castle.Core.Internal;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using Mediachase.Commerce.Catalog;

namespace EPiServer.Reference.Commerce.Site.Features.Cart.Controllers
{
    [Authorize]
    public class WishListController : PageController<WishListPage>
    {
        private readonly IContentLoader _contentLoader;
        private readonly ICartService _cartService;
        private ICart _wishlist;
        private readonly IOrderRepository _orderRepository;
        readonly CartViewModelFactory _cartViewModelFactory;
        private readonly IQuickOrderService _quickOrderService;
        private readonly ReferenceConverter _referenceConverter;

        public WishListController(
            IContentLoader contentLoader,
            ICartService cartService,
            IOrderRepository orderRepository,
            CartViewModelFactory cartViewModelFactory,
            IQuickOrderService quickOrderService,
            ReferenceConverter referenceConverter)
        {
            _contentLoader = contentLoader;
            _cartService = cartService;
            _orderRepository = orderRepository;
            _cartViewModelFactory = cartViewModelFactory;
            _quickOrderService = quickOrderService;
            _referenceConverter = referenceConverter;
        }

        [HttpGet]
        public ActionResult Index(WishListPage currentPage)
        {
            var viewModel = _cartViewModelFactory.CreateWishListViewModel(WishList);
            viewModel.CurrentPage = currentPage;

            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult WishListMiniCartDetails()
        {
            var viewModel = _cartViewModelFactory.CreateWishListMiniCartViewModel(WishList);
            return PartialView("_WishListMiniCartDetails", viewModel);
        }

        [HttpPost]
        [AllowDBWrite]
        public ActionResult AddToCart(string code)
        {
            string warningMessage = string.Empty;

            ModelState.Clear();

            if (WishList == null)
            {
                _wishlist = _cartService.LoadOrCreateCart(_cartService.DefaultWishListName);
            }

            if(WishList.GetAllLineItems().Any(item => item.Code.Equals(code, StringComparison.OrdinalIgnoreCase)))
            {
                return WishListMiniCartDetails();
            }

            if (_cartService.AddToCart(WishList, code, out warningMessage))
            {
                _orderRepository.Save(WishList);
                return WishListMiniCartDetails();
            }

            // HttpStatusMessage can't be longer than 512 characters.
            warningMessage = warningMessage.Length < 512 ? warningMessage : warningMessage.Substring(512);
            return new HttpStatusCodeResult(500, warningMessage);
        }

        [HttpPost]
        [AllowDBWrite]
        public JsonResult AddVariantsToOrderPad(List<string> variants)
        {
            var returnedMessages = new List<string>();

            ModelState.Clear();

            if (WishList == null)
            {
                _wishlist = _cartService.LoadOrCreateCart(_cartService.DefaultWishListName);
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
                    if (_cartService.AddToCart(WishList, sku, out warningMessage))
                    {
                        _orderRepository.Save(WishList);
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

            return Json(returnedMessages, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowDBWrite]
        public ActionResult ChangeCartItem(string code, decimal quantity, string size, string newSize)
        {
            ModelState.Clear();

            _cartService.ChangeCartItem(WishList, 0, code, quantity, size, newSize);
            _orderRepository.Save(WishList);
            return WishListMiniCartDetails();
        }

        [HttpPost]
        [AllowDBWrite]
        public ActionResult DeleteWishList()
        {
            if (WishList != null)
            {
                _orderRepository.Delete(WishList.OrderLink);
            }
            var startPage = _contentLoader.Get<StartPage>(ContentReference.StartPage);

            return RedirectToAction("Index", new { Node = startPage.WishListPage });
        }

        private ICart WishList
        {
            get { return _wishlist ?? (_wishlist = _cartService.LoadCart(_cartService.DefaultWishListName)); }
        }
    }
}