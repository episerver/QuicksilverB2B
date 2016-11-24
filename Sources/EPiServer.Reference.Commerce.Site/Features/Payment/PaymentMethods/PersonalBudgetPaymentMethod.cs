//using EPiServer.Commerce.Order;
//using EPiServer.Framework.Localization;
//using EPiServer.ServiceLocation;
//using Mediachase.Commerce.Orders;

//namespace EPiServer.Reference.Commerce.Site.Features.Payment.PaymentMethods
//{
//    public class PersonalBudgetPaymentMethod : PaymentMethodBase
//    {
//        private readonly IOrderFactory _orderFactory;

//        public PersonalBudgetPaymentMethod()
//            : this(LocalizationService.Current, ServiceLocator.Current.GetInstance<IOrderFactory>())
//        {
//        }

//        public PersonalBudgetPaymentMethod(IOrderFactory orderFactory)
//            : this(LocalizationService.Current, orderFactory)
//        {
//        }

//        public PersonalBudgetPaymentMethod(LocalizationService localizationService, IOrderFactory orderFactory)
//            : base(localizationService)
//        {
//            _orderFactory = orderFactory;
//        }

//        public override bool ValidateData()
//        {
//            return true;
//        }
        
//        public override IPayment CreatePayment(decimal amount)
//        {
//            var payment = _orderFactory.CreatePayment();
//            payment.PaymentMethodId = PaymentMethodId;
//            payment.PaymentMethodName = "PersonalBudget";
//            payment.Amount = amount;
//            payment.Status = PaymentStatus.Pending.ToString();
//            payment.TransactionType = TransactionType.Authorization.ToString();
//            return payment;
//        }

//        public override void PostProcess(IPayment payment)
//        {
//            payment.Status = PaymentStatus.Processed.ToString();
//        }
//    }
//}