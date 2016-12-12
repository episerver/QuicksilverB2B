using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using EPiServer.Reference.Commerce.Site.Features.Product.Models;

namespace Episerver.DataImporter.Models
{
    [CatalogContentType(DisplayName = "GenericVariation", GUID = "ba18ca7a-e74b-46fa-91ed-1957253ab81f", Description = "")]
    public class GenericVariation : BaseVariant
    {
        [Searchable]
        [Tokenize]
        [CultureSpecific]
        [IncludeInDefaultSearch]
        [BackingType(typeof(PropertyString))]
        [Display(Name = "Brand", Order = 1)]
        public virtual string Brand { get; set; }

        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [Display(Name = "Description", Order = 2)]
        public virtual XhtmlString Description { get; set; }

        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [Display(Name = "Long Description", Order = 3)]
        public virtual XhtmlString LongDescription { get; set; }

        [CultureSpecific]
        [Display(Name = "Product Teaser", Order = 4)]
        public virtual XhtmlString ProductTeaser { get; set; }

        [BackingType(typeof(PropertyString))]
        [Display(Name = "ModelNumber", Order = 5)]
        public virtual string ModelNumber { get; set; }

        [BackingType(typeof(PropertyString))]
        [Display(Name = "UPC", Order = 6)]
        public virtual string Upc { get; set; }
    }
}