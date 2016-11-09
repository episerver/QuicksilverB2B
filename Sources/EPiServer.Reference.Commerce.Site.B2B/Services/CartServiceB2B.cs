using System;
using System.Linq;
using EPiServer.Commerce.Order;
using EPiServer.Logging;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Website.Helpers;

namespace EPiServer.Reference.Commerce.Site.B2B.Services
{
    [ServiceConfiguration(typeof(ICartServiceB2B), Lifecycle = ServiceInstanceScope.Singleton)]
    public class CartServiceB2B: ICartServiceB2B
    {
        private readonly IOrderRepository _orderRepository;
        private const string DefaultCartName = "Default";

        public CartServiceB2B(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
 
        public ICart CreateNewCart()
        {
            return new CartHelper(DefaultCartName).Cart;
        }

        public void DeleteCart(ICart cart)
        {
            _orderRepository.Delete(cart.OrderLink);
        }
        public void RemoveQuoteNumber(ICart cart)
        {
            if (cart == null || cart.GetAllLineItems().Any()) return;
            if (cart.Properties["ParentOrderGroupId"] == null) return;

            cart.Properties["ParentOrderGroupId"] = 0;
            _orderRepository.Save(cart);
        }
        public bool PlaceCartForQuote(ICart cart)
        {
            var quoteResult = true;
            try
            {
                OrderReference orderReference = _orderRepository.SaveAsPurchaseOrder(cart);
                PurchaseOrder purchaseOrder = _orderRepository.Load<IPurchaseOrder>(orderReference.OrderGroupId) as PurchaseOrder;
                if (purchaseOrder != null)
                {
                    purchaseOrder["QuoteExpireDate"] = DateTime.Now.AddDays(30);
                    purchaseOrder["PreQuoteTotal"] = purchaseOrder.Total;
                    purchaseOrder["QuoteStatus"] = "RequestQuotation";
                    purchaseOrder.Status = OrderStatus.OnHold.ToString();
                }
                _orderRepository.Save(purchaseOrder);
            }
            catch (Exception ex)
            {
                quoteResult = false;
                LogManager.GetLogger(GetType()).Error("Failed to process request quotation request.", ex);
            }

            return quoteResult;
        }

        public ICart PlaceOrderToCart(IPurchaseOrder purchaseOrder, ICart cart)
        {
            ICart returnCart = cart;
            var lineItems = purchaseOrder.GetAllLineItems();
            foreach (var lineItem in lineItems)
            {
                cart.AddLineItem(lineItem);
            }
            return returnCart;
        }
    }
}
