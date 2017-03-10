using EPiServer.Commerce.Order;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Orders;

namespace EPiServer.Reference.Commerce.Site.Features.Payment.PaymentMethods
{
    public class CashOnDeliveryPaymentMethod : PaymentMethodBase
    {
        private readonly IOrderGroupFactory _orderGroupFactory;

        public CashOnDeliveryPaymentMethod()
            : this(LocalizationService.Current, ServiceLocator.Current.GetInstance<IOrderGroupFactory>())
        {
        }

        public CashOnDeliveryPaymentMethod(IOrderGroupFactory orderGroupFactory)
            : this(LocalizationService.Current, orderGroupFactory)
        {
        }

        public CashOnDeliveryPaymentMethod(LocalizationService localizationService, IOrderGroupFactory orderGroupFactory)
            : base(localizationService)
        {
            _orderGroupFactory = orderGroupFactory;
        }

        public override bool ValidateData()
        {
            return true;
        }
        
        public override IPayment CreatePayment(IOrderGroup orderGroup, decimal amount)
        {
            var payment = _orderGroupFactory.CreatePayment(orderGroup);
            payment.PaymentMethodId = PaymentMethodId;
            payment.PaymentMethodName = "CashOnDelivery";
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