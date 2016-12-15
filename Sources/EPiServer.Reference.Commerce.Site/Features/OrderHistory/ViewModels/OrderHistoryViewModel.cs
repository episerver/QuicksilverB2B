using System.Collections.Generic;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.OrderHistory.Pages;
using EPiServer.Reference.Commerce.Site.Features.Shared.ViewModels;

namespace EPiServer.Reference.Commerce.Site.Features.OrderHistory.ViewModels
{
    public class OrderHistoryViewModel : PageViewModel<OrderHistoryPage>
    {
        public List<OrderViewModel> Orders { get; set; }
        public string OrderDetailsPageUrl { get; set; }
        public ContactViewModel CurrentCustomer { get; set; }
    }
}