using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using Mediachase.Commerce;

namespace EPiServer.Reference.Commerce.Site.Features.Cart.ViewModels
{
    public class MiniCartViewModel 
    {
        public ContentReference CheckoutPage { get; set; }

        public decimal ItemCount { get; set; }

        public IEnumerable<ShipmentViewModel> Shipments { get; set; }

        public Money Total { get; set; }
        public bool IsQuotedCart { get; set; }
        public ContactViewModel CurrentCustomer { get; set; }
        public string QuoteStatus { get; set; }
    }
}