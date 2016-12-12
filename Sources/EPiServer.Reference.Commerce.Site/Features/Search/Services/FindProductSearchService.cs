using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Catalog;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using EPiServer.Find.Commerce;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework;
using EPiServer.Reference.Commerce.Site.B2B;
using EPiServer.Reference.Commerce.Site.Features.Market.Services;
using EPiServer.Reference.Commerce.Site.Features.Product.Models;
using EPiServer.Reference.Commerce.Site.Features.Product.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Search.Models;
using EPiServer.Reference.Commerce.Site.Features.Search.ViewModels;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Mediachase.Commerce;

namespace EPiServer.Reference.Commerce.Site.Features.Search.Services
{
    [ServiceConfiguration(typeof(IFindProductSearchService), Lifecycle = ServiceInstanceScope.PerRequest)]
    public class FindProductSearchService : IFindProductSearchService
    {
        private readonly ICurrentMarket _currentMarket;
        private readonly ICurrencyService _currencyService;
        private readonly AssetUrlResolver _assetUrlResolver;
        private readonly UrlResolver _urlResolver;
        private static readonly int _defaultPageSize = 18;
        protected const int MaxNumberOfFacets = 50;

        public FindProductSearchService(ICurrentMarket currentMarket, ICurrencyService currencyService, AssetUrlResolver assetUrlResolver, UrlResolver urlResolver)
        {
            _currentMarket = currentMarket;
            _currencyService = currencyService;
            _assetUrlResolver = assetUrlResolver;
            _urlResolver = urlResolver;
        }

        public CustomSearchResult SearchProducts(IContent currentContent, FilterOptionViewModel filterOptions)
        {
            if (filterOptions == null)
            {
                return new CustomSearchResult
                {
                    FacetGroups = new List<FacetGroupOption>(),
                    ProductViewModels = new List<ProductViewModel>()
                };
            }
            var query = BuildSearchQuery(currentContent, filterOptions);
            var searchResults = query.GetContentResult();

            return new CustomSearchResult
            {
                FacetGroups = BuildFacetGroupList(currentContent, filterOptions),
                ProductViewModels = CreateProductViewModels(searchResults),
                TotalCount = searchResults.TotalMatching
            };
        }

        private IEnumerable<ProductViewModel> CreateProductViewModels(IContentResult<FashionProduct> searchResult)
        {
            var market = _currentMarket.GetCurrentMarket();
            var currency = _currencyService.GetCurrentCurrency();
            return searchResult.Select(product =>
            {
                var originalPrice = product.OriginalPrices.FirstOrDefault(
                    p => p.MarketId == market.MarketId && p.UnitPrice.Currency == currency);
                var listingPrice = product.ListingPrices.FirstOrDefault(
                    p => p.MarketId == market.MarketId && p.UnitPrice.Currency == currency);
                return new ProductViewModel
                {
                    Brand = product.Brand,
                    Code = product.Code,
                    DisplayName = product.DisplayName,
                    PlacedPrice = originalPrice?.UnitPrice ?? new Money(0, currency),
                    DiscountedPrice = listingPrice?.UnitPrice ?? new Money(0, currency),
                    ImageUrl = _assetUrlResolver.GetAssetUrl<IContentImage>(product),
                    Url = _urlResolver.GetUrl(product.ContentLink),
                    IsAvailable = originalPrice != null && originalPrice.UnitPrice.Amount > 0
                };
            });
        }

        private ITypeSearch<FashionProduct> BuildSearchQuery(IContent currentContent, FilterOptionViewModel filterOptions)
        {
            var query = BuildBaseQuery(currentContent, filterOptions);

            ProductSortOrder sortOrder;
            if (!Enum.TryParse(filterOptions.Sort, out sortOrder))
            {
                sortOrder = ProductSortOrder.PriceAsc;
            }
            switch (sortOrder)
            {
                case ProductSortOrder.PriceAsc:
                    var market = _currentMarket.GetCurrentMarket();
                    var currency = _currencyService.GetCurrentCurrency();
                    query =
                        query.OrderBy(
                            x =>
                                x.ListingPrices, p => p.UnitPrice.Amount,
                            p => p.MarketId.Match(market.MarketId) & p.UnitPrice.Currency.Match(currency));
                    break;
                case ProductSortOrder.NewestFirst:
                    query = query.OrderByDescending(x => x.Created);
                    break;
            }

            var pageSize = filterOptions.PageSize > 0 ? filterOptions.PageSize : _defaultPageSize;
            var page = filterOptions.Page > 0 ? filterOptions.Page - 1 : 0;
            query = query.Skip(page * pageSize).Take(pageSize);
            return query;
        }

        private ITypeSearch<FashionProduct> BuildBaseQuery(IContent currentContent, FilterOptionViewModel filterOptions)
        {
            var query = SearchClient.Instance.Search<FashionProduct>();
            if (!string.IsNullOrWhiteSpace(filterOptions.Q))
            {
                query = query.For(filterOptions.Q);
            }
            query = query.FilterOnCurrentMarket();
            query = query.FilterForVisitor();

            var nodeContent = currentContent as NodeContent;
            if (nodeContent != null)
            {
                query =
                    query.Filter(
                        p =>
                            p.Ancestors()
                                .Match(nodeContent.ContentLink.ToReferenceWithoutVersion().ToString()));
            }

            FilterBuilder<FashionProduct> filter = SearchClient.Instance.BuildFilter<FashionProduct>();
            foreach (var facetGroup in filterOptions.FacetGroups.Where(fg => fg.Facets.Any(fo => fo.Selected)))
            {
                var selectedValues = facetGroup.Facets.Where(fo => fo.Selected).Select(fo => fo.Key);
                switch (facetGroup.GroupFieldName)
                {
                    case Constants.Product.Brand:
                        filter = filter.And(p => p.Brand.In(selectedValues));
                        break;
                    case Constants.Product.AvailableColors:
                        filter = filter.And(p => p.AvailableColors.In(selectedValues));
                        break;
                    case Constants.Product.AvailableSizes:
                        filter = filter.And(p => p.AvailableSizes.In(selectedValues));
                        break;
                    case Constants.Product.TopCategory:
                        filter = filter.And(p => p.TopCategory.In(selectedValues));
                        break;
                    case Constants.Product.Categories:
                        filter = filter.And(p => p.ParentName.In(selectedValues));
                        break;
                }
            }
            query = query.Filter(filter);
            return query;
        }

        private IEnumerable<FacetGroupOption> BuildFacetGroupList(IContent currentContent, FilterOptionViewModel filterOptions)
        {
            var query = BuildBaseQuery(currentContent, filterOptions);
            query = query.Take(0);
            query = query.TermsFacetFor(p => p.ParentName, r => r.Size = MaxNumberOfFacets);
            query = query.TermsFacetFor(p => p.AvailableColors, r => r.Size = MaxNumberOfFacets);
            query = query.TermsFacetFor(p => p.AvailableSizes, r => r.Size = MaxNumberOfFacets);
            query = query.TermsFacetFor(p => p.Brand, r => r.Size = MaxNumberOfFacets);
            query = query.TermsFacetFor(p => p.TopCategory, r => r.Size = MaxNumberOfFacets);
            IContentResult<FashionProduct> results = query.GetContentResult();

            var facetGroups = new List<FacetGroupOption>();

            AddFacet(facetGroups, results.TermsFacetFor(p => p.ParentName),
                filterOptions.FacetGroups.FirstOrDefault(fg => fg.GroupFieldName.Equals(Constants.Product.Categories)),
                Constants.Product.Categories);
            AddFacet(facetGroups, results.TermsFacetFor(p => p.AvailableColors),
                filterOptions.FacetGroups.FirstOrDefault(fg => fg.GroupFieldName.Equals(Constants.Product.AvailableColors)),
                Constants.Product.AvailableColors);
            AddFacet(facetGroups, results.TermsFacetFor(p => p.AvailableSizes),
                filterOptions.FacetGroups.FirstOrDefault(fg => fg.GroupFieldName.Equals(Constants.Product.AvailableSizes)),
                Constants.Product.AvailableSizes);
            AddFacet(facetGroups, results.TermsFacetFor(p => p.Brand),
                filterOptions.FacetGroups.FirstOrDefault(fg => fg.GroupFieldName.Equals(Constants.Product.Brand)),
                Constants.Product.Brand);
            AddFacet(facetGroups, results.TermsFacetFor(p => p.TopCategory),
                filterOptions.FacetGroups.FirstOrDefault(fg => fg.GroupFieldName.Equals(Constants.Product.TopCategory)),
                Constants.Product.TopCategory);
            
            return facetGroups;
        }

        private void AddFacet(List<FacetGroupOption> facetGroups, TermsFacet termsFacet, FacetGroupOption facetGroupOption, string facetGroup)
        {
            var facet = GetFacet(termsFacet, facetGroupOption, facetGroup);
            if (facet != null)
            {
                facetGroups.Add(facet);
            }
        }

        private FacetGroupOption GetFacet(TermsFacet termsFacet, FacetGroupOption facetGroupOption, string facetGroup)
        {
            if (!termsFacet.Any(f => f != null && f.Count > 0)) return null;

            var termFacetGroup = new FacetGroupOption
            {
                GroupFieldName = facetGroup,
                GroupName = facetGroup,
                Facets = new List<FacetOption>()
            };
            foreach (TermCount term in termsFacet)
            {
                if (term.Count > 0)
                {
                    termFacetGroup.Facets.Add(new FacetOption
                    {
                        Name = term.Term,
                        Selected = facetGroupOption != null && facetGroupOption.Facets.Any(fo => fo.Selected && fo.Key.Equals(term.Term)),
                        Count = term.Count,
                        Key = term.Term
                    });
                }
            }
            return termFacetGroup;
        }
    }
}
