using System.Collections.Generic;
using EPiServer.Core;

namespace EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels
{
    public class OrdersPageViewModel
    {
        public List<OrderOrganizationViewModel> OrdersOrganization { get; set; }
        public PageData CurrentPage { get; set; }
    }
}
