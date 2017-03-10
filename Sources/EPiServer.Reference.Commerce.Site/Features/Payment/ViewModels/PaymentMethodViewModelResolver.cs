using System;
using EPiServer.Commerce.Order;
using EPiServer.Reference.Commerce.Site.Features.Payment.PaymentMethods;
using EPiServer.ServiceLocation;

namespace EPiServer.Reference.Commerce.Site.Features.Payment.ViewModels
{
    public class PaymentMethodViewModelResolver
    {
        private static readonly Lazy<IOrderGroupFactory> _orderGroupFactory = new Lazy<IOrderGroupFactory>(() => ServiceLocator.Current.GetInstance<IOrderGroupFactory>());

        public static IPaymentMethodViewModel<PaymentMethodBase> Resolve(string paymentMethodName, IOrderGroupFactory orderGroupFactory)
        {
            switch (paymentMethodName)
            {
                case "CashOnDelivery":
                    return new CashOnDeliveryViewModel { PaymentMethod = new CashOnDeliveryPaymentMethod(orderGroupFactory) };
                case "GenericCreditCard":
                    return new GenericCreditCardViewModel { PaymentMethod = new GenericCreditCardPaymentMethod(orderGroupFactory) };
                case "BudgetPayment":
                    return new BudgetPaymentViewModel { PaymentMethod = new BudgetPaymentMethod(orderGroupFactory) };
            }

            throw new ArgumentException("No view model has been implemented for the method " + paymentMethodName, "paymentMethodName");
        }

        public static IPaymentMethodViewModel<PaymentMethodBase> Resolve(string paymentMethodName)
        {
            return Resolve(paymentMethodName, _orderGroupFactory.Value);
        }
    }
}