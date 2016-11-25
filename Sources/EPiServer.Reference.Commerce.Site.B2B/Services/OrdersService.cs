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
        public List<OrderOrganizationViewModel> GetUserOrders(Guid userGuid)
        {
            var iPurchaseOrders = _orderRepository.Load<IPurchaseOrder>(userGuid)
                                 .OrderByDescending(x => x.Created)
                                 .ToList();
            var ordersOrganization = new List<OrderOrganizationViewModel>();

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
                    SubOrganization = "",
                    IsPaymentApproved = false
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
                var budgetPayment = GetOrderBudgetPayment(purchaseOrder);
                orderViewModel.IsOrganizationOrder = budgetPayment != null;
                if (budgetPayment != null)
                    orderViewModel.IsPaymentApproved = orderViewModel.IsOrganizationOrder && budgetPayment.TransactionType.Equals(TransactionType.Capture.ToString());

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
            return ordersOrganization.Where(order => order.IsOrganizationOrder).ToList();
        }

        public IPayment GetOrderBudgetPayment(IPurchaseOrder purchaseOrder)
        {
            if (purchaseOrder?.Forms == null || !purchaseOrder.Forms.Any())
            {
                return null;
            }

            return
                purchaseOrder.Forms.Where(orderForm => orderForm.Payments != null && orderForm.Payments.Any())
                    .SelectMany(orderForm => orderForm.Payments)
                    .FirstOrDefault(payment => payment.PaymentMethodName.Equals(Constants.Order.BudgetPayment));
        }

        public void ApproveOrder(int orderGroupId)
        {
            var purchaseOrder = _orderRepository.Load<PurchaseOrder>(orderGroupId);
            if (purchaseOrder == null) return;

            var budgetPayment = GetOrderBudgetPayment(purchaseOrder) as Payment;
            if (budgetPayment == null) return;

            budgetPayment.TransactionType = TransactionType.Capture.ToString();
            budgetPayment.Status = PaymentStatus.Pending.ToString();
            budgetPayment.AcceptChanges();
            purchaseOrder.ProcessPayments();
        }
    }
}
