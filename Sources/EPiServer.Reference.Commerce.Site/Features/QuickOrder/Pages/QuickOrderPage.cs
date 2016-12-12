using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Reference.Commerce.Site.Features.QuickOrder.Blocks;

namespace EPiServer.Reference.Commerce.Site.Features.QuickOrder.Pages
{
    [ContentType(DisplayName = "Quick Order Page", GUID = "9F846F7D-2DFA-4983-815D-C09B12CEF993", Description = "", AvailableInEditMode = true)]
    public class QuickOrderPage : PageData
    {
        [CultureSpecific]
        [Display(
            Name = "Main body",
            Description = "The main body will be shown in the main content area of the page, using the XHTML-editor you can insert for example text, images and tables.",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual XhtmlString MainBody { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Quick Order Block Content Area",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 2)]
        [AllowedTypes(typeof(QuickOrderBlock))]
        public virtual ContentArea QuickOrderBlockContentArea { get; set; }
    }
}