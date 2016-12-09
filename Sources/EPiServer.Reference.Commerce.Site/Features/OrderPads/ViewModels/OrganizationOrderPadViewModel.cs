using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.Reference.Commerce.Site.Features.OrderPads.ViewModels
{
    public class OrganizationOrderPadViewModel
    {
        public List<UsersOrderPadViewModel> UsersOrderPad { get; set; } 
        public string OrganizationName { get; set; }
        public string OrganizationId { get; set; }
    }
}