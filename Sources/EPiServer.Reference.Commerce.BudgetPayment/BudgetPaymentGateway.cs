using System.Collections.Generic;
using System.Linq;
using EPiServer.Reference.Commerce.Site.B2B;
using EPiServer.Reference.Commerce.Site.B2B.Enums;
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
        public override bool ProcessPayment(Payment payment, ref string message)
        {
            if (payment?.Parent?.Parent == null)
            {
                message = "Failed to process your payment.";
                return false;
            }

            var customer = _customerService.Service.GetContactById(payment.Parent.Parent.CustomerId.ToString());
            if (customer == null || customer.Role != B2BUserRoles.Purchaser)
            {
                message = "Failed to process your payment.";
                return false;
            }

            var budget = _budgetService.Service.GetCustomerCurrentBudget(customer.Organization.OrganizationId, customer.ContactId);
            if (budget == null || budget.RemainingBudget < payment.Amount)
            {
                message = "Insufficient budget.";
                return false;
            }

            if (payment.TransactionType == TransactionType.Capture.ToString())
            {
                UpdateUserBudgets(customer, payment.Amount);
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
    }
}
