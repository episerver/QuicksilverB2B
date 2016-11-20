using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;

namespace EPiServer.Reference.Commerce.Site.Features.QuickOrder.ViewModels
{
    public class QuickOrderPageViewModel 
    {
        public PageData CurrentPage { get; set; }
        public List<ProductViewModel> ProductsList { get; set; }
        public List<string> ReturnedMessages  { get; set; }
    }
}