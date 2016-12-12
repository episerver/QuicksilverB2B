using System.Collections.Generic;
using EPiServer.Reference.Commerce.Site.B2B.Models.Search;

namespace EPiServer.Reference.Commerce.Site.Features.Search.Services
{
    public interface IEPiFindSearchService
    {
        IEnumerable<UserSearchResultModel> SearchUsers(string query);
        IEnumerable<SkuSearchResultModel> SearchSkus(string query);
    }
}