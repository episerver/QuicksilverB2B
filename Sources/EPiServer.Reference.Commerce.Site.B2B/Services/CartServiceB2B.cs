using System;
using System.Linq;
using EPiServer.Commerce.Order;
using EPiServer.Logging;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Website.Helpers;
using System.Configuration;
using Mediachase.Commerce.Customers;

namespace EPiServer.Reference.Commerce.Site.B2B.Services
{
    [ServiceConfiguration(typeof(ICartServiceB2B), Lifecycle = ServiceInstanceScope.Singleton)]
    public class CartServiceB2B: ICartServiceB2B
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrganizationService _organizationService;
        private const string DefaultCartName = "Default";

        public CartServiceB2B(IOrderRepository orderRepository, IOrganizationService organizationService)
        {
            _orderRepository = orderRepository;
            _organizationService = organizationService;
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
                foreach (var lineItem in cart.GetFirstForm().GetAllLineItems())
                {
                    lineItem.Properties[Constants.Quote.PreQuotePrice] = lineItem.PlacedPrice;
                }
                
                OrderReference orderReference = _orderRepository.SaveAsPurchaseOrder(cart);
                PurchaseOrder purchaseOrder = _orderRepository.Load<IPurchaseOrder>(orderReference.OrderGroupId) as PurchaseOrder;
                if (purchaseOrder != null)
                {
                    int quoteExpireDays;
                    int.TryParse(ConfigurationManager.AppSettings[Constants.Quote.QuoteExpireDate], out quoteExpireDays);
                    purchaseOrder[Constants.Quote.QuoteExpireDate] =
                        string.IsNullOrEmpty(ConfigurationManager.AppSettings[Constants.Quote.QuoteExpireDate])
                            ? DateTime.Now.AddDays(30)
                            : DateTime.Now.AddDays(quoteExpireDays);

                    purchaseOrder[Constants.Quote.PreQuoteTotal] = purchaseOrder.Total;
                    purchaseOrder[Constants.Quote.QuoteStatus] = Constants.Quote.RequestQuotation;
                    purchaseOrder.Status = OrderStatus.OnHold.ToString();
                    if (string.IsNullOrEmpty(purchaseOrder[Constants.Customer.CustomerFullName].ToString()))
                    {
                        if (CustomerContext.Current != null && CustomerContext.Current.CurrentContact != null)
                        {
                            var contact = CustomerContext.Current.CurrentContact;
                            purchaseOrder[Constants.Customer.CustomerFullName] = contact.FullName;
                            purchaseOrder[Constants.Customer.CustomerEmailAddress] = contact.Email;
                            if (_organizationService.GetCurrentUserOrganization() != null)
                            {
                                var organization = _organizationService.GetCurrentUserOrganization();
                                purchaseOrder[Constants.Customer.CurrentCustomerOrganization] = organization.Name;
                            }
                        }
                    }
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
