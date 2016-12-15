using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Order;
using EPiServer.Reference.Commerce.Site.B2B;
using EPiServer.Reference.Commerce.Site.B2B.Enums;
using EPiServer.Reference.Commerce.Site.B2B.Extensions;
using EPiServer.Reference.Commerce.Site.B2B.Models.Entities;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Plugins.Payment;

namespace EPiServer.Reference.Commerce.BudgetPayment
{
    public class BudgetPaymentGateway : AbstractPaymentGateway
    {
        private static Injected<IBudgetService> _budgetService;
        private static Injected<ICustomerService> _customerService;
        private static Injected<IOrdersService> _ordersService;
        public override bool ProcessPayment(Payment payment, ref string message)
        {
            if (payment?.Parent?.Parent == null)
            {
                message = "Failed to process your payment.";
                return false;
            }

            var currentOrder = payment.Parent.Parent;
            var customer = _customerService.Service.GetContactById(currentOrder.CustomerId.ToString());
            if (customer == null)
            {
                message = "Failed to process your payment.";
                return false;
            }
            var isQuoteOrder = currentOrder.IsQuoteCart();
            if (isQuoteOrder)
            {
                if (customer.Role != B2BUserRoles.Approver)
                {
                    message = "Failed to process your payment.";
                    return false;
                }
            }
            else if (customer.Role != B2BUserRoles.Purchaser)
            {
                message = "Failed to process your payment.";
                return false;
            }

            var purchaserCustomer = !isQuoteOrder ? customer : _ordersService.Service.GetPurchaserCustomer(currentOrder);
            if (AreBudgetsOnHold(purchaserCustomer))
            {
                message = "Budget on hold.";
                return false;
            }

            var budget = _budgetService.Service.GetCustomerCurrentBudget(purchaserCustomer.Organization.OrganizationId, purchaserCustomer.ContactId);
            if (budget == null || budget.RemainingBudget < payment.Amount)
            {
                message = "Insufficient budget.";
                return false;
            }

            if (payment.TransactionType == TransactionType.Capture.ToString())
            {
                UpdateUserBudgets(purchaserCustomer, payment.Amount);
                payment.Status = PaymentStatus.Processed.ToString();
                payment.AcceptChanges();
            }
            return true;
        }

        private void UpdateUserBudgets(ContactViewModel customer, decimal amount)
        {
            var budgetsToUpdate = new List<Budget>
            {
                _budgetService.Service.GetCurrentOrganizationBudget(customer.Organization.OrganizationId),
                _budgetService.Service.GetCurrentOrganizationBudget(customer.Organization.ParentOrganizationId),
                _budgetService.Service.GetCustomerCurrentBudget(customer.Organization.OrganizationId,
                    customer.ContactId)
            };
            if (budgetsToUpdate.All(budget => budget == null)) return;

            foreach (var budget in budgetsToUpdate)
            {
                budget.SpentBudget += amount;
                budget.SaveChanges();
            }
        }

        private bool AreBudgetsOnHold(ContactViewModel customer)
        {
            if (customer?.Organization == null) return true;

            var budgetsToCheck = new List<Budget>
            {
                _budgetService.Service.GetCurrentOrganizationBudget(customer.Organization.OrganizationId),
                _budgetService.Service.GetCurrentOrganizationBudget(customer.Organization.ParentOrganizationId),
                _budgetService.Service.GetCustomerCurrentBudget(customer.Organization.OrganizationId,
                    customer.ContactId)
            };
            return budgetsToCheck.Any(budget => budget.Status.Equals(Constants.BudgetStatus.OnHold));
        }
    }
}
