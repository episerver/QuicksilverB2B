using System.ComponentModel.DataAnnotations;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Reference.Commerce.Site.B2B.Extensions;
using EPiServer.Shell.ObjectEditing;

namespace EPiServer.Reference.Commerce.Site.Features.Search.Models
{
    public class BaseNode : NodeContent
    {
        [SelectMany(SelectionFactoryType = typeof(OrganizationSelectionFactory))]
        [Display(Name = "ACLBlackList", Order = 6)]
        public virtual string ACLBlackList { get; set; }
    }
}