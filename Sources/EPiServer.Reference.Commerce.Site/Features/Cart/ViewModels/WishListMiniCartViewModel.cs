using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;

namespace EPiServer.Reference.Commerce.Site.Features.Cart.ViewModels
{
    public class WishListMiniCartViewModel : CartViewModelBase
    {
        public ContentReference WishListPage { get; set; }
        public ContactViewModel CurrentCustomer { get; set; }
    }
}