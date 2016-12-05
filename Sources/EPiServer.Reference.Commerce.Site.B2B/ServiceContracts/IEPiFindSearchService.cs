using System.Collections.Generic;
using EPiServer.Reference.Commerce.Site.B2B.Models.Search;

namespace EPiServer.Reference.Commerce.Site.B2B.ServiceContracts
{
    public interface IEPiFindSearchService
    {
        IEnumerable<UserSearchResultModel> SearchUsers(string query);
    }
}