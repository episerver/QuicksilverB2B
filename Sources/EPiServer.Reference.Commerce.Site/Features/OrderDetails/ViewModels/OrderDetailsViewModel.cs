using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Order;
using EPiServer.Reference.Commerce.Site.Features.OrderDetails.Pages;
using EPiServer.Reference.Commerce.Site.Features.Shared.Models;
using EPiServer.Reference.Commerce.Site.Features.Shared.ViewModels;
using Mediachase.Commerce.Orders;

namespace EPiServer.Reference.Commerce.Site.Features.OrderDetails.ViewModels
{
    public class OrderDetailsViewModel : PageViewModel<OrderDetailsPage>
    {
        public IPurchaseOrder PurchaseOrder { get; set; }
        public IEnumerable<OrderDetailsItemViewModel> Items { get; set; }
        public AddressModel BillingAddress { get; set; }
        public IList<AddressModel> ShippingAddresses { get; set; }
        public string QuoteStatus { get; set; }
        public int OrderGroupId { get; set; }
        public IPayment BudgetPayment { get; set; }
        public bool IsPaymentApproved => IsOrganizationOrder && BudgetPayment.TransactionType.Equals(TransactionType.Capture.ToString());
        public bool IsOrganizationOrder => BudgetPayment != null;
    }
}