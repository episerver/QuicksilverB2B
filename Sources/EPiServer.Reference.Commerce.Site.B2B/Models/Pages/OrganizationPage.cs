using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;

namespace EPiServer.Reference.Commerce.Site.B2B.Models.Pages
{
    [ContentType(DisplayName = "OrganizationPage", GUID = "e50f0e69-0851-40dc-b00c-38f0acec3f32", Description = "", AvailableInEditMode = true)]
    public class OrganizationPage : PageData
    {

        [CultureSpecific]
        [Display(
            Name = "Main body",
            Description = "The main body will be shown in the main content area of the page, using the XHTML-editor you can insert for example text, images and tables.",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual XhtmlString MainBody { get; set; }

    }
}