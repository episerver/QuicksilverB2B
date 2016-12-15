using System;
using EPiServer.Commerce.Order;

namespace EPiServer.Reference.Commerce.Site.B2B.ServiceContracts
{
    public interface ICartServiceB2B
    {
        ICart CreateNewCart();
        void DeleteCart(ICart cart);
        bool PlaceCartForQuote(ICart cart);
        ICart PlaceOrderToCart(IPurchaseOrder purchaseOrder, ICart cart);
        void RemoveQuoteNumber(ICart cart);
        string DefaultWishListName { get; }
        int PlaceCartForQuoteById(int orderId, Guid userId);
    }
}
