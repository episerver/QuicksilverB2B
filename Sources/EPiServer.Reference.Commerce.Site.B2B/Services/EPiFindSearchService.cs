using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework;
using EPiServer.Reference.Commerce.Site.B2B.Enums;
using EPiServer.Reference.Commerce.Site.B2B.Models.Contact;
using EPiServer.Reference.Commerce.Site.B2B.Models.Search;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Customers;

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
    }
}