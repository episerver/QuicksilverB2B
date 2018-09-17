using System.Linq;
using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.B2B;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.Reference.Commerce.Site.Features.Cart.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Market.Services;
using EPiServer.Reference.Commerce.Site.Features.Start.Pages;
using EPiServer.ServiceLocation;
using Mediachase.Commerce;

namespace EPiServer.Reference.Commerce.Site.Features.Cart.ViewModelFactories
{
    [ServiceConfiguration(typeof(CartViewModelFactory), Lifecycle = ServiceInstanceScope.Singleton)]
    public class CartViewModelFactory 
    {
        private readonly IContentLoader _contentLoader;
        private readonly ICurrencyService _currencyService;
        readonly IOrderGroupCalculator _orderGroupCalculator;
        readonly ICustomerService _customerService;
        readonly ShipmentViewModelFactory _shipmentViewModelFactory;

        public CartViewModelFactory(
            IContentLoader contentLoader, 
            ICurrencyService currencyService, 
            IOrderGroupCalculator orderGroupCalculator,
            ICustomerService customerService,
            ShipmentViewModelFactory shipmentViewModelFactory)
        {
            _contentLoader = contentLoader;
            _currencyService = currencyService;
            _orderGroupCalculator = orderGroupCalculator;
            _shipmentViewModelFactory = shipmentViewModelFactory;
            _customerService = customerService;
        }

        public virtual MiniCartViewModel CreateMiniCartViewModel(ICart cart)
        {
            if (cart == null)
            {
                return new MiniCartViewModel
                {
                    ItemCount = 0,
                    CheckoutPage = _contentLoader.Get<StartPage>(ContentReference.StartPage).CheckoutPage,
                    Shipments = Enumerable.Empty<ShipmentViewModel>(),
                    Total = new Money(0, _currencyService.GetCurrentCurrency()),
                    CurrentCustomer = _customerService.GetCurrentContact(),
                    IsQuotedCart = false
                };
            }

            // If order comes from a quoted order.
            var quotedCart = false;
            if (cart.Properties[Constants.Quote.ParentOrderGroupId] != null)
            {
                int orderLink = int.Parse(cart.Properties[Constants.Quote.ParentOrderGroupId].ToString());
                if (orderLink != 0)
                {
                    quotedCart = true;
                }
            }
            return new MiniCartViewModel
            {
                ItemCount = GetLineItemsTotalQuantity(cart),
                CheckoutPage = _contentLoader.Get<StartPage>(ContentReference.StartPage).CheckoutPage,
                Shipments = _shipmentViewModelFactory.CreateShipmentsViewModel(cart),
                Total = _orderGroupCalculator.GetSubTotal(cart),
                IsQuotedCart = quotedCart,
                CurrentCustomer = _customerService.GetCurrentContact()
            };
        }

        public virtual LargeCartViewModel CreateLargeCartViewModel(ICart cart)
        {
            if (cart == null)
            {
                var zeroAmount = new Money(0, _currencyService.GetCurrentCurrency());

                return new LargeCartViewModel
                {
                    Shipments = Enumerable.Empty<ShipmentViewModel>(),
                    TotalDiscount = zeroAmount,
                    Total = zeroAmount,
                };
            }
            // If order comes from a quoted order.
            var quotedCart = false;
            if (cart.Properties[Constants.Quote.ParentOrderGroupId] != null)
            {
                int orderLink = int.Parse(cart.Properties[Constants.Quote.ParentOrderGroupId].ToString());
                if (orderLink != 0)
                {
                    quotedCart = true;
                }
            }
            return new LargeCartViewModel
            {
               Shipments = _shipmentViewModelFactory.CreateShipmentsViewModel(cart),
               TotalDiscount = new Money(cart.GetAllLineItems().Sum(x => x.GetEntryDiscount()), cart.Currency),
               Total = _orderGroupCalculator.GetSubTotal(cart),
               IsQuotedCart = quotedCart
            };
        }

        public virtual WishListViewModel CreateWishListViewModel(ICart cart)
        {
            if (cart == null)
            {
                return new WishListViewModel
                {
                    ItemCount = 0,
                    CartItems = new CartItemViewModel[0],
                    Total = new Money(0, _currencyService.GetCurrentCurrency()),
                    CurrentCustomer = _customerService.GetCurrentContact(),
                };
            }

            return new WishListViewModel
            {
                ItemCount = GetLineItemsTotalQuantity(cart),
                CartItems = _shipmentViewModelFactory.CreateShipmentsViewModel(cart).SelectMany(x => x.CartItems),
                Total = _orderGroupCalculator.GetSubTotal(cart),
                CurrentCustomer = _customerService.GetCurrentContact(),
            };
        }

        public virtual WishListMiniCartViewModel CreateWishListMiniCartViewModel(ICart cart)
        {
            if (cart == null)
            {
                return new WishListMiniCartViewModel
                {
                    ItemCount = 0,
                    WishListPage = _contentLoader.Get<StartPage>(ContentReference.StartPage).WishListPage,
                    CartItems = new CartItemViewModel[0],
                    Total = new Money(0, _currencyService.GetCurrentCurrency()),
                    CurrentCustomer = _customerService.GetCurrentContact(),
                };
            }

            return new WishListMiniCartViewModel
            {
                ItemCount = GetLineItemsTotalQuantity(cart),
                WishListPage = _contentLoader.Get<StartPage>(ContentReference.StartPage).WishListPage,
                CartItems = _shipmentViewModelFactory.CreateShipmentsViewModel(cart).SelectMany(x => x.CartItems),
                Total = _orderGroupCalculator.GetSubTotal(cart),
                CurrentCustomer = _customerService.GetCurrentContact(),
            };
        }

        private decimal GetLineItemsTotalQuantity(ICart cart)
        {
            return cart.GetAllLineItems().Sum(x => x.Quantity);
        }
    }
}