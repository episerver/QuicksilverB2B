using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Commerce.SpecializedProperties;
using EPiServer.Core;
using System.ComponentModel.DataAnnotations;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace EPiServer.Reference.Commerce.Site.Features.Product.Models
{
    [CatalogContentType(GUID = "550ebcfc-c989-4272-8f94-c6d079f56181",
        MetaClassName = "FashionProduct",
        DisplayName = "Fashion product",
        Description = "Display fashion product")]
    public class FashionProduct : BaseProduct
    {
        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [Display(Name = "Sizing", Order = 4)]
        public virtual XhtmlString Sizing { get; set; }

        [CultureSpecific]
        [Display(Name = "Product Teaser", Order = 5)]
        public virtual XhtmlString ProductTeaser { get; set; }

        [Searchable]
        [IncludeInDefaultSearch]
        [BackingType(typeof(PropertyDictionaryMultiple))]
        [Display(Name = "Available Sizes", Order = 6)]
        public virtual ItemCollection<string> AvailableSizes { get; set; }

        [Searchable]
        [IncludeInDefaultSearch]
        [BackingType(typeof(PropertyDictionaryMultiple))]
        [Display(Name = "Available Colors", Order = 6)]
        public virtual ItemCollection<string> AvailableColors { get; set; }
		
        [Display(Name = "Content Area",
        Description = "Content area",
        GroupName = SystemTabNames.Content,
        Order = 20)]
        public virtual ContentArea ContentArea { get; set; }

        [Ignore]
        public override ItemCollection<string> AvailableSizeList => AvailableSizes;

        [Ignore]
        public override ItemCollection<string> AvailableColorList => AvailableColors;
    }
}