using EPiServer.Commerce.Order;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Cart.Pages;
using Mediachase.Commerce.Customers;

namespace EPiServer.Reference.Commerce.Site.Features.Cart.ViewModels
{
    public class WishListViewModel : CartViewModelBase 
    {
        public WishListPage CurrentPage { get; set; }
        public ContactViewModel CurrentCustomer { get; set; }
    }
}