using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Order;
using EPiServer.Logging;
using EPiServer.Reference.Commerce.Site.B2B.DomainServiceContracts;
using EPiServer.Reference.Commerce.Site.B2B.Extensions;
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
        private readonly ICustomerDomainService _customerDomainService;

        public OrdersService(IOrderRepository orderRepository, ICustomerDomainService customerDomainService)
        {
            _orderRepository = orderRepository;
            _customerDomainService = customerDomainService;
        }

        public List<OrderOrganizationViewModel> GetUserOrders(Guid userGuid)
        {
            var purchaseOrders = OrderContext.Current.GetPurchaseOrders(userGuid)
                                             .OrderByDescending(x => x.Created)
                                             .ToList();
            var ordersOrganization = new List<OrderOrganizationViewModel>();

            foreach (var purchaseOrder in purchaseOrders)
            {
                var orderViewModel = new OrderOrganizationViewModel
                {
                    OrderNumber = purchaseOrder.TrackingNumber,
                    OrderGroupId = purchaseOrder.OrderGroupId,
                    PlacedOrderDate = purchaseOrder.Created.ToString("yyyy MMMM dd"),
                    Ammount = purchaseOrder.GetTotal().Amount.ToString("N"),
                    Currency = purchaseOrder.BillingCurrency,
                    User = "",
                    Status = purchaseOrder.Status,
                    SubOrganization = "",
                    IsPaymentApproved = false
                };
                if (purchaseOrder[Constants.Customer.CurrentCustomerOrganization] != null)
                {
                    orderViewModel.SubOrganization =
                        purchaseOrder[Constants.Customer.CurrentCustomerOrganization].ToString();
                }
                if (purchaseOrder[Constants.Customer.CustomerFullName] != null)
                {
                    orderViewModel.User =
                        purchaseOrder[Constants.Customer.CustomerFullName].ToString();
                }

                if (!string.IsNullOrEmpty(purchaseOrder[Constants.Quote.QuoteStatus]?.ToString()) &&
                        (purchaseOrder.Status == OrderStatus.InProgress.ToString() || purchaseOrder.Status == OrderStatus.OnHold.ToString()))
                {
                    orderViewModel.Status = purchaseOrder[Constants.Quote.QuoteStatus].ToString();
                    DateTime quoteExpireDate;
                    DateTime.TryParse(purchaseOrder[Constants.Quote.QuoteExpireDate].ToString(), out quoteExpireDate);
                    if (DateTime.Compare(DateTime.Now, quoteExpireDate) > 0)
                    {
                        orderViewModel.Status = Constants.Quote.QuoteExpired;
                    }
                    orderViewModel.IsQuoteOrder = true;
                }
                var budgetPayment = GetOrderBudgetPayment(purchaseOrder);
                orderViewModel.IsOrganizationOrder = budgetPayment != null || orderViewModel.IsQuoteOrder;
                if (budgetPayment != null)
                {
                    orderViewModel.IsPaymentApproved = orderViewModel.IsOrganizationOrder && budgetPayment.TransactionType.Equals(TransactionType.Capture.ToString());
                    orderViewModel.Status = orderViewModel.IsPaymentApproved
                        ? orderViewModel.Status
                        : Constants.Order.PendingApproval;
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

        public bool ApproveOrder(int orderGroupId)
        {
            var purchaseOrder = _orderRepository.Load<PurchaseOrder>(orderGroupId);
            if (purchaseOrder == null) return false;

            var budgetPayment = GetOrderBudgetPayment(purchaseOrder) as Payment;
            if (budgetPayment == null) return false;

            try
            {
                budgetPayment.TransactionType = TransactionType.Capture.ToString();
                budgetPayment.Status = PaymentStatus.Pending.ToString();
                budgetPayment.AcceptChanges();
                purchaseOrder.ProcessPayments();
                budgetPayment.Status = PaymentStatus.Processed.ToString();
                budgetPayment.AcceptChanges();
            }
            catch (Exception ex)
            {
                budgetPayment.TransactionType = TransactionType.Authorization.ToString();
                budgetPayment.Status = PaymentStatus.Processed.ToString();
                budgetPayment.AcceptChanges();
                LogManager.GetLogger(GetType()).Error("Failed processs on approve order.", ex);
                return false;
            }
            return true;

        }

        public ContactViewModel GetPurchaserCustomer(OrderGroup order)
        {
            if (order == null)
            {
                return null;
            }
            if (!order.IsQuoteCart())
            {
                return new ContactViewModel(_customerDomainService.GetContactById(order.CustomerId.ToString()));
            }

            var parentOrder = _orderRepository.Load<PurchaseOrder>(order.GetParentOrderId());
            return parentOrder != null
                ? new ContactViewModel(_customerDomainService.GetContactById(parentOrder.CustomerId.ToString()))
                : null;
        }

    }
}
