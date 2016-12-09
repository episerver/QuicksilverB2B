using System.Collections.Generic;
using EPiServer.Commerce.Order;

namespace EPiServer.Reference.Commerce.Site.Features.OrderPads.ViewModels
{
    public class UsersOrderPadViewModel
    {
        public ICart WishCartList { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
    }
}