using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Commerce.SpecializedProperties;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Reference.Commerce.Site.Features.Shared.Extensions;

namespace EPiServer.Reference.Commerce.Site.Features.Product.Models
{
    public class BaseProduct : ProductContent
    {
        [Searchable]
        [CultureSpecific]
        [Tokenize]
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

        [Ignore]
        public virtual ItemCollection<string> AvailableSizeList { get; set; }
        [Ignore]
        public virtual ItemCollection<string> AvailableColorList { get; set; }

        [Ignore]
        public virtual string ParentName => this.ParentName();

        [Ignore]
        public virtual string TopCategory => this.TopCategory();

        [Ignore]
        public virtual List<Price> OriginalPrices => this.OriginalPrices();

        [Ignore]
        public virtual List<Price> ListingPrices => this.ListingPrices();
    }
}