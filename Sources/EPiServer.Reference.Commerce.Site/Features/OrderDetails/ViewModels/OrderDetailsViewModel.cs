using System.Collections.Generic;
using EPiServer.Commerce.Order;
using EPiServer.Reference.Commerce.Site.B2B;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.OrderDetails.Pages;
using EPiServer.Reference.Commerce.Site.Features.Shared.Models;
using EPiServer.Reference.Commerce.Site.Features.Shared.ViewModels;
using Mediachase.Commerce.Orders;

namespace EPiServer.Reference.Commerce.Site.Features.OrderDetails.ViewModels
{
    public class OrderDetailsViewModel : PageViewModel<OrderDetailsPage>
    {
        public ContactViewModel CurrentCustomer { get; set; }
        public IPurchaseOrder PurchaseOrder { get; set; }
        public IEnumerable<OrderDetailsItemViewModel> Items { get; set; }
        public AddressModel BillingAddress { get; set; }
        public IList<AddressModel> ShippingAddresses { get; set; }
        public string QuoteStatus { get; set; }
        public int OrderGroupId { get; set; }
        public IPayment BudgetPayment { get; set; }

        public string OrderStatus
            =>
                !IsPaymentApproved
                    ? Constants.Order.PendingApproval
                    : QuoteStatus ?? PurchaseOrder.OrderStatus.ToString();

        public bool IsPaymentApproved
            =>
                BudgetPayment == null ||
                (BudgetPayment != null && BudgetPayment.TransactionType.Equals(TransactionType.Capture.ToString()));
        public bool IsOrganizationOrder => BudgetPayment != null || !string.IsNullOrEmpty(QuoteStatus);
    }
}