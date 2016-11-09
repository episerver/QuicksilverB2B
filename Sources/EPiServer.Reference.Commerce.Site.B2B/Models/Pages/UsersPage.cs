using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;

namespace EPiServer.Reference.Commerce.Site.B2B.Models.Pages
{
    [ContentType(DisplayName = "UsersPage", GUID = "8118b44f-17d9-47af-a40c-c77d1aa0d2ae", Description = "", AvailableInEditMode = true)]
    public class UsersPage : PageData
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