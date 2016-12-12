using System.Collections.Generic;
using System.Web.Mvc;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.Features.Product.Models;
using Mediachase.Commerce;

namespace EPiServer.Reference.Commerce.Site.Features.Product.ViewModels
{
    public class BaseProductViewModel
    {
        public BaseProduct Product { get; set; }
        public Money? DiscountedPrice { get; set; }
        public Money ListingPrice { get; set; }
        public BaseVariant Variation { get; set; }
        public IList<SelectListItem> Colors { get; set; }
        public IList<SelectListItem> Sizes { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public IList<string> Images { get; set; }
        public bool IsAvailable { get; set; }

        public NodeContent CategoryPage { get; set; }
        public NodeContent SubcategoryPage { get; set; }

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
        public List<VariantViewModel> Variants { get; set; }
        public List<string> ReturnedMessages { get; set; }
        public ContentArea ContentArea { get; set; }
    }
}