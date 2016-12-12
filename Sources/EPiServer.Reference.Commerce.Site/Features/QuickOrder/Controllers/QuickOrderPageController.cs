using System.Web.Mvc;
using EPiServer.Reference.Commerce.Site.Features.QuickOrder.Pages;
using EPiServer.Reference.Commerce.Site.Features.QuickOrder.ViewModels;
using EPiServer.Web.Mvc;

namespace EPiServer.Reference.Commerce.Site.Features.QuickOrder.Controllers
{
    [Authorize]
    public class QuickOrderPageController : PageController<QuickOrderPage>
    {
        public ActionResult Index(QuickOrderPage currentPage)
        {
            return View(new QuickOrderPageViewModel
            {
                CurrentPage = currentPage
            });
        }
    }
}