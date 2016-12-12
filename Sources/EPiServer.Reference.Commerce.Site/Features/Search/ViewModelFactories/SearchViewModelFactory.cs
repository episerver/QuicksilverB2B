using System.Linq;
using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using Mediachase.Search;
using EPiServer.Reference.Commerce.Site.Features.Search.Services;
using EPiServer.Reference.Commerce.Site.Features.Search.ViewModels;
using Mediachase.Commerce;

namespace EPiServer.Reference.Commerce.Site.Features.Search.ViewModelFactories
{
    public class SearchViewModelFactory
    {
        private readonly ISearchService _searchService;
        private readonly LocalizationService _localizationService;
        private readonly ICurrentMarket _currentMarket;
        private readonly IFindProductSearchService _findProductSearchService;

        public SearchViewModelFactory(LocalizationService localizationService, ISearchService searchService, ICurrentMarket currentMarket, IFindProductSearchService findProductSearchService)
        {
            _searchService = searchService;
            _localizationService = localizationService;
            _currentMarket = currentMarket;
            _findProductSearchService = findProductSearchService;
        }

        public virtual SearchViewModel<T> Create<T>(T currentContent, FilterOptionViewModel viewModel) where T : IContent
        {
            if (viewModel.Q != null && (viewModel.Q.StartsWith("*") || viewModel.Q.StartsWith("?")))
            {
                return new SearchViewModel<T>
                {
                    CurrentContent = currentContent,
                    FilterOption = viewModel,
                    HasError = true,
                    ErrorMessage = _localizationService.GetString("/Search/BadFirstCharacter")
                };
            }

            var customSearchResult = _findProductSearchService.SearchProducts(currentContent, viewModel);

            viewModel.TotalCount = customSearchResult.TotalCount;
            viewModel.FacetGroups = customSearchResult.FacetGroups.ToList();

            viewModel.Sorting = _searchService.GetSortOrder().Select(x => new SelectListItem
            {
                Text = _localizationService.GetString("/Category/Sort/" + x.Name),
                Value = x.Name.ToString(),
                Selected = string.Equals(x.Name.ToString(), viewModel.Sort)
            });

            return new SearchViewModel<T>
            {
                CurrentContent = currentContent,
                ProductViewModels = customSearchResult.ProductViewModels,
                Facets = customSearchResult.SearchResult != null ? customSearchResult.SearchResult.FacetGroups : new ISearchFacetGroup[0],
                FilterOption = viewModel
            };
        }

    }
}