using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.Features.Navigation.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Start.Pages;
using EPiServer.SpecializedProperties;
using System.Web.Mvc;

namespace EPiServer.Reference.Commerce.Site.Features.Navigation.Controllers
{
    public class B2BNavigationController : Controller
    {
        private readonly IContentLoader _contentLoader;

        public B2BNavigationController(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public ActionResult Index(IContent currentContent)
        {
            var startPage = _contentLoader.Get<StartPage>(ContentReference.StartPage);
           
            var viewModel = new NavigationViewModel
            {
                StartPage = startPage,
                CurrentContentLink = currentContent?.ContentLink,
                UserLinks = new LinkItemCollection()
            };

            if (startPage.B2BMenu != null)
            {
                viewModel.UserLinks.AddRange(startPage.B2BMenu);
            }

            return PartialView(viewModel);
        }
    }
}