using EPiServer.Commerce.Order;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Orders;

namespace EPiServer.Reference.Commerce.Site.Features.Payment.PaymentMethods
{
    public class BudgetPaymentMethod : PaymentMethodBase
    {
        private readonly IOrderGroupFactory _orderGroupFactory;

        public BudgetPaymentMethod()
            : this(LocalizationService.Current, ServiceLocator.Current.GetInstance<IOrderGroupFactory>())
        {
        }

        public BudgetPaymentMethod(IOrderGroupFactory orderGroupFactory)
            : this(LocalizationService.Current, orderGroupFactory)
        {
        }

        public BudgetPaymentMethod(LocalizationService localizationService, IOrderGroupFactory orderFactory)
            : base(localizationService)
        {
            _orderGroupFactory = orderFactory;
        }

        public override bool ValidateData()
        {
            return true;
        }

        public override IPayment CreatePayment(IOrderGroup orderGroup, decimal amount)
        {
            var payment = _orderGroupFactory.CreatePayment(orderGroup);
            payment.PaymentMethodId = PaymentMethodId;
            payment.PaymentMethodName = "BudgetPayment";
            payment.Amount = amount;
            payment.Status = PaymentStatus.Pending.ToString();
            payment.TransactionType = TransactionType.Authorization.ToString();
            return payment;
        }

        public override void PostProcess(IPayment payment)
        {
            payment.Status = PaymentStatus.Processed.ToString();
        }
    }
}