using EPiServer.Commerce;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Episerver.DataImporter.Models
{
    [ContentType(DisplayName = "GenericImage", GUID = "b7177be9-e764-4755-90bb-75c0fc54735e", Description = "")]
    //[MediaDescriptor(ExtensionString = "jpg,jpeg,jpe,ico,gif,bmp,png")]
    public class GenericImage : ImageData
    {
        [CultureSpecific]
        [Display(
            Name = "Alternate text",
            Description = "Description of the image",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual string Description { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Link",
            Description = "Link to content",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        [UIHint(UIHint.AllContent)]
        public virtual ContentReference Link { get; set; }
    }
}