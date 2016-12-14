using EPiServer.Reference.Commerce.Site.Features.Search.Pages;
using EPiServer.Reference.Commerce.Site.Features.Search.Services;
using EPiServer.Reference.Commerce.Site.Features.Search.ViewModelFactories;
using EPiServer.Reference.Commerce.Site.Features.Search.ViewModels;
using EPiServer.Web.Mvc;
using System.Web.Mvc;

namespace EPiServer.Reference.Commerce.Site.Features.Search.Controllers
{
    public class SearchController : PageController<SearchPage>
    {
        private readonly SearchViewModelFactory _viewModelFactory;
        private readonly IFindProductSearchService _findProductSearchService;

        public SearchController(SearchViewModelFactory viewModelFactory, IFindProductSearchService findProductSearchService)
        {
            _viewModelFactory = viewModelFactory;
            _findProductSearchService = findProductSearchService;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Index(SearchPage currentPage, FilterOptionViewModel viewModel)
        {
            var model = _viewModelFactory.Create(currentPage, viewModel);

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult QuickSearch(string q = "")
        {
            var result = _findProductSearchService.QuickSearch(q);
            return View(result);
        }
    }
}