using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework;
using EPiServer.Reference.Commerce.Site.B2B.Models.Search;
using EPiServer.Reference.Commerce.Site.Features.Market.Services;
using EPiServer.Reference.Commerce.Site.Features.Shared.Services;
using EPiServer.ServiceLocation;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Core;
using Mediachase.Commerce.Pricing;

namespace EPiServer.Reference.Commerce.Site.Features.Search.Services
{
    [ServiceConfiguration(typeof(IEPiFindSearchService), Lifecycle = ServiceInstanceScope.Transient)]
    public class EPiFindSearchService : IEPiFindSearchService
    {
        private readonly IPriceService _priceService;
        private readonly IPromotionService _promotionService;
        private readonly ICurrentMarket _currentMarket;
        private readonly ICurrencyService _currencyservice;

        public EPiFindSearchService(
            IPriceService priceService,
            IPromotionService promotionService,
            ICurrentMarket currentMarket,
            ICurrencyService currencyservice)
        {
            _priceService = priceService;
            _promotionService = promotionService;
            _currentMarket = currentMarket;
            _currencyservice = currencyservice;
        }

        public IEnumerable<UserSearchResultModel> SearchUsers(string query)
        {
            var searchResults = SearchClient.Instance.Search<UserSearchResultModel>().For(query).GetResult();
            if (searchResults != null && searchResults.Any())
                return searchResults.Hits.AsEnumerable().Select(x => x.Document);
            return Enumerable.Empty<UserSearchResultModel>();
        }

        public IEnumerable<SkuSearchResultModel> SearchSkus(string query)
        {
            var market = _currentMarket.GetCurrentMarket();
            var currency = _currencyservice.GetCurrentCurrency();
            
            var searchQuery = SearchClient.Instance.Search<VariationContent>();
            searchQuery = searchQuery.For(query).FilterForVisitor();
            var searchResults = searchQuery.GetContentResult();

            if (searchResults != null && searchResults.Any())
            {
                var searchResult = searchResults.Items;
                return searchResult.Select(variation =>
                {
                    var defaultPrice = _priceService.GetDefaultPrice(market.MarketId, DateTime.Now,
                        new CatalogKey(variation.Code), currency);
                    var discountedPrice = defaultPrice != null ? _promotionService.GetDiscountPrice(defaultPrice.CatalogKey, market.MarketId,
                        currency) : null;
                    return new SkuSearchResultModel
                    {
                        Sku = variation.Code,
                        ProductName = variation.DisplayName,
                        UnitPrice = discountedPrice?.UnitPrice.Amount ?? 0
                    };
                });
            }
            return Enumerable.Empty<SkuSearchResultModel>();
        }
    }
}