using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Plugins.Payment;

namespace EPiServer.Reference.Commerce.BudgetPayment
{
    public class BudgetPaymentGateway : AbstractPaymentGateway
    {
        private static Injected<IBudgetService> _budgetService;
        public override bool ProcessPayment(Payment payment, ref string message)
        {
            if (payment?.Parent?.Parent == null) return false;

            var budget = _budgetService.Service.GetUserActiveBudget(payment.Parent.Parent.CustomerId);
            
            return true;
        }
    }
}
