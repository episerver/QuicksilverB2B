using System.Collections.Generic;
using System.Linq;
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
            if (customer == null)
            {
                message = "Failed to process your payment.";
                return false;
            }

            var budget = GetApplicableBudget(customer);
            if (budget == null || budget.RemainingBudget < payment.Amount)
            {
                message = "Insufficient budget.";
                return false;
            }

            if (payment.TransactionType == TransactionType.Capture.ToString() ||
                (customer.Role == B2BUserRoles.Admin || customer.Role == B2BUserRoles.Approver))
            {
                UpdateUserBudgets(customer, payment.Amount);
            }
            return true;
        }

        private Budget GetApplicableBudget(ContactViewModel customer)
        {
            switch (customer.Role)
            {
                case B2BUserRoles.Admin:
                case B2BUserRoles.Approver:
                    return _budgetService.Service.GetCurrentOrganizationBudget(customer.Organization.OrganizationId);
                case B2BUserRoles.Purchaser:
                    return _budgetService.Service.GetCustomerCurrentBudget(customer.Organization.OrganizationId,
                        customer.ContactId);
                default:
                    return null;
            }
        }

        private void UpdateUserBudgets(ContactViewModel customer, decimal amount)
        {
            var budgetsToUpdate = new List<Budget>
            {
                _budgetService.Service.GetCurrentOrganizationBudget(customer.Organization.OrganizationId)
            };
            switch (customer.Role)
            {
                case B2BUserRoles.Approver:
                case B2BUserRoles.Purchaser:
                    budgetsToUpdate.Add(_budgetService.Service.GetCurrentOrganizationBudget(customer.Organization.ParentOrganizationId));
                    budgetsToUpdate.Add(
                        _budgetService.Service.GetCustomerCurrentBudget(customer.Organization.OrganizationId,
                            customer.ContactId));
                    break;
            }
            if (!budgetsToUpdate.Any()) return;

            foreach (var budget in budgetsToUpdate)
            {
                budget.SpentBudget += amount;
                budget.SaveChanges();
            }
        }
    }
}
