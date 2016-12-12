using EPiServer.Reference.Commerce.Site.Features.Search.ViewModelFactories;
using EPiServer.Reference.Commerce.Site.Features.Search.ViewModels;
using EPiServer.Web.Mvc;
using System.Web.Mvc;
using Episerver.DataImporter.Models;

namespace EPiServer.Reference.Commerce.Site.Features.Search.Controllers
{
    public class GenericNodeController : ContentController<GenericNode>
    {
        private readonly SearchViewModelFactory _viewModelFactory;

        public GenericNodeController(SearchViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ViewResult Index(GenericNode currentContent, FilterOptionViewModel viewModel)
        {
            var model = _viewModelFactory.Create(currentContent, viewModel);
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult Facet(GenericNode currentContent, FilterOptionViewModel viewModel)
        {
            var model = _viewModelFactory.Create(currentContent, viewModel);
            return PartialView("_Facet", viewModel);
        }
    }
}