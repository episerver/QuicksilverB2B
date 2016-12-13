using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using EPiServer.Reference.Commerce.Site.Features.Search.Models;

namespace Episerver.DataImporter.Models
{
    [CatalogContentType(DisplayName = "GenericNode", GUID = "4ac27ad4-bf60-4ea0-9a77-28a89d38d3fd", Description = "")]
    public class GenericNode : BaseNode
    {
        [CultureSpecific]
        [Display(Name = "LongName", GroupName = SystemTabNames.Content)]
        [BackingType(typeof(PropertyString))]
        public virtual string LongName { get; set; }

        [CultureSpecific]
        [Display(Name = "Teaser", GroupName = SystemTabNames.Content)]
        public virtual string Teaser { get; set; }

        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [Display(Name = "Description", GroupName = SystemTabNames.Content)]
        public virtual XhtmlString Description { get; set; }

    }
}