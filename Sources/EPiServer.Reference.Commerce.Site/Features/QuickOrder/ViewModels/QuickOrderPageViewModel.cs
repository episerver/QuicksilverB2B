using System.Collections.Generic;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.QuickOrder.Pages;
using EPiServer.Reference.Commerce.Site.Features.Shared.ViewModels;

namespace EPiServer.Reference.Commerce.Site.Features.QuickOrder.ViewModels
{
    public class QuickOrderPageViewModel : PageViewModel<QuickOrderPage>
    {
        public List<ProductViewModel> ProductsList { get; set; }
        public List<string> ReturnedMessages  { get; set; }
    }
}