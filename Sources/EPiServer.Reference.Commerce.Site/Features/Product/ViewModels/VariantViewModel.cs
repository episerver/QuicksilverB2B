using Mediachase.Commerce;

namespace EPiServer.Reference.Commerce.Site.Features.Product.ViewModels
{
    public class VariantViewModel
    {
        public string ImageUrl { get; set; }
        public Money? DiscountedPrice { get; set; }
        public Money ListingPrice { get; set; }
        public string Size { get; set; }
        public string Sku { get; set; }

        public Money SavePrice
        {
            get
            {

                if (DiscountedPrice.HasValue && DiscountedPrice.Value < ListingPrice)
                {
                    return ListingPrice - DiscountedPrice.Value;
                }
                return new Money(0, ListingPrice.Currency);
            }
        }
    }
}