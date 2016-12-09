using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework;
using EPiServer.Reference.Commerce.Site.B2B.Models.Search;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.ServiceLocation;

namespace EPiServer.Reference.Commerce.Site.B2B.Services
{
    [ServiceConfiguration(typeof(IEPiFindSearchService), Lifecycle = ServiceInstanceScope.PerRequest)]
    public class EPiFindSearchService : IEPiFindSearchService
    {
        public IEnumerable<UserSearchResultModel> SearchUsers(string query)
        {
            var searchResults = SearchClient.Instance.Search<UserSearchResultModel>().For(query).GetResult();
            if (searchResults != null && searchResults.Any())
                return searchResults.Hits.AsEnumerable().Select(x => x.Document);
            return Enumerable.Empty<UserSearchResultModel>();
        }

        public IEnumerable<SkuSearchResultModel> SearchSkus(string query)
        {
            var searchResults = SearchClient.Instance.Search<VariationContent>().For(query).GetContentResult();
            if (searchResults != null && searchResults.Any())
            {
                var searchResult = searchResults.Items;
                return searchResult.Select(product => new SkuSearchResultModel
                {
                    Sku = product.Code,
                    ProductName = product.DisplayName,
                    UnitPrice = product.GetDefaultPrice().UnitPrice.Amount
                });
            }
            return Enumerable.Empty<SkuSearchResultModel>();
        }
    }
}