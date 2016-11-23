using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Order;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Orders;

namespace EPiServer.Reference.Commerce.Site.B2B.Services
{
    [ServiceConfiguration(typeof(IOrdersService), Lifecycle = ServiceInstanceScope.Singleton)]
    public class OrdersService : IOrdersService
    {
        private readonly IOrderRepository _orderRepository;
        public OrdersService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public void GetUserOrders(Guid userGuid, out List<OrderOrganizationViewModel> ordersOrganization)
        {
            var iPurchaseOrders = _orderRepository.Load<IPurchaseOrder>(userGuid)
                                 .OrderByDescending(x => x.Created)
                                 .ToList();
            ordersOrganization = new List<OrderOrganizationViewModel>();

            foreach (var purchaseOrder in iPurchaseOrders)
            {
                var orderViewModel = new OrderOrganizationViewModel
                {
                    OrderNumber = purchaseOrder.OrderNumber,
                    OrderGroupId = purchaseOrder.OrderLink.OrderGroupId,
                    PlacedOrderDate = purchaseOrder.Created.ToString("yyyy MMMM dd"),
                    Ammount = purchaseOrder.GetTotal().Amount.ToString("F"),
                    Currency = purchaseOrder.Currency.CurrencyCode,
                    User = "",
                    Status = purchaseOrder.OrderStatus.ToString(),
                    SubOrganization = ""
                };

                if (purchaseOrder.Properties[Constants.Customer.CurrentCustomerOrganization] != null)
                {
                    orderViewModel.SubOrganization =
                        purchaseOrder.Properties[Constants.Customer.CurrentCustomerOrganization].ToString();
                }
                if (purchaseOrder.Properties[Constants.Customer.CustomerFullName] != null)
                {
                    orderViewModel.User =
                        purchaseOrder.Properties[Constants.Customer.CustomerFullName].ToString();
                }
                if (!string.IsNullOrEmpty(purchaseOrder.Properties[Constants.Quote.QuoteStatus]?.ToString()) &&
                        (purchaseOrder.OrderStatus == OrderStatus.InProgress || purchaseOrder.OrderStatus == OrderStatus.OnHold))
                {
                    orderViewModel.Status = purchaseOrder.Properties[Constants.Quote.QuoteStatus].ToString();
                    DateTime quoteExpireDate;
                    DateTime.TryParse(purchaseOrder.Properties[Constants.Quote.QuoteExpireDate].ToString(), out quoteExpireDate);
                    if (DateTime.Compare(DateTime.Now, quoteExpireDate) > 0)
                    {
                        orderViewModel.Status = Constants.Quote.QuoteExpired;
                    }
                }

                ordersOrganization.Add(orderViewModel);
            }
        }

    }

}
