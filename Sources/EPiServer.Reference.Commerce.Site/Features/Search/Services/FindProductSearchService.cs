using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using EPiServer.Commerce.Catalog;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Api;
using EPiServer.Find.Commerce;
using EPiServer.Find.Api.Querying.Queries;
using EPiServer.Find.Commerce.Services;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework;
using EPiServer.Globalization;
using EPiServer.Reference.Commerce.Site.Features.Market.Services;
using EPiServer.Reference.Commerce.Site.Features.Product.Models;
using EPiServer.Reference.Commerce.Site.Features.Product.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Search.Models;
using EPiServer.Reference.Commerce.Site.Features.Search.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Shared.Extensions;
using EPiServer.Reference.Commerce.Site.Infrastructure.Indexing;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Mediachase.Commerce;
using SortOrder = EPiServer.Reference.Commerce.Site.Features.Search.Models.SortOrder;

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
                ProductViewModels = CreateProductViewModels(searchResults)
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
            query = query.FilterOnCurrentMarket().StaticallyCacheFor(TimeSpan.FromSeconds(1));
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
            return query;
        }

        private IEnumerable<FacetGroupOption> BuildFacetGroupList(IContent currentContent, FilterOptionViewModel filterOptions)
        {
            var query = BuildBaseQuery(currentContent, filterOptions);

            return Enumerable.Empty<FacetGroupOption>();
        }
    }
}
