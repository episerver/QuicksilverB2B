using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Commerce.Order;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.OrderPads.Pages;
using EPiServer.Reference.Commerce.Site.Features.Shared.ViewModels;

namespace EPiServer.Reference.Commerce.Site.Features.OrderPads.ViewModels
{
    public class OrderPadsPageViewModel : PageViewModel<OrderPadsPage>
    {
        public string QuoteStatus { get; set; }
        public ContactViewModel CurrentCustomer { get; set; }
        public List<ICart> OrderPardCartsList { get; set; } 
        public List<OrganizationOrderPadViewModel> OrganizationOrderPadList {get; set;} 
        
    }
}