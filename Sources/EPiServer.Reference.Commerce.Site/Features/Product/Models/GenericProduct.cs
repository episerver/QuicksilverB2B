using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using EPiServer.Commerce.SpecializedProperties;
using EPiServer.Reference.Commerce.Site.Features.Product.Models;

namespace Episerver.DataImporter.Models
{
    [CatalogContentType(DisplayName = "GenericProduct", GUID = "f0f19ae6-e4a3-4b7c-88eb-9dbcfb52d9e8", Description = "")]
    public class GenericProduct : BaseProduct
    {
        [CultureSpecific]
        [Display(Name = "Product Teaser", Order = 4)]
        public virtual XhtmlString ProductTeaser { get; set; }

        [BackingType(typeof(PropertyString))]
        [Display(Name = "ModelNumber", Order = 5)]
        public virtual string ModelNumber { get; set; }

        [BackingType(typeof(PropertyString))]
        [Display(Name = "UPC", Order = 6)]
        public virtual string Upc { get; set; }

        [Searchable]
        [Tokenize]
        [IncludeInDefaultSearch]
        [BackingType(typeof(PropertyString))]
        [Display(Name = "Size", Order = 7)]
        public virtual string Size { get; set; }

        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [BackingType(typeof(PropertyString))]
        [Display(Name = "Color", Order = 8)]
        public virtual string Color { get; set; }

        [Ignore]
        public override ItemCollection<string> AvailableSizeList
            => string.IsNullOrEmpty(Size) ? new ItemCollection<string>() : new ItemCollection<string>(Size.Split('|'));

        [Ignore]
        public override ItemCollection<string> AvailableColorList
            => string.IsNullOrEmpty(Color) ? new ItemCollection<string>() : new ItemCollection<string>(Color.Split('|'));
    }
}