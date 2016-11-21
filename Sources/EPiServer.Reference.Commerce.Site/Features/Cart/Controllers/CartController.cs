using System.Linq;
using EPiServer.Commerce.Order;
using EPiServer.Reference.Commerce.Site.Features.Cart.Services;
using EPiServer.Reference.Commerce.Site.Features.Cart.ViewModelFactories;
using EPiServer.Reference.Commerce.Site.Infrastructure.Attributes;
using System.Web.Mvc;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.Reference.Commerce.Site.B2B;

namespace EPiServer.Reference.Commerce.Site.Features.Cart.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private ICart _cart;
        private readonly IOrderRepository _orderRepository;
        readonly CartViewModelFactory _cartViewModelFactory;
        private readonly ICartServiceB2B _cartServiceB2B;

        public CartController(
            ICartService cartService,
            IOrderRepository orderRepository,
            CartViewModelFactory cartViewModelFactory,
            ICartServiceB2B cartServiceB2B)
        {
            _cartService = cartService;
            _orderRepository = orderRepository;
            _cartViewModelFactory = cartViewModelFactory;
            _cartServiceB2B = cartServiceB2B;
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