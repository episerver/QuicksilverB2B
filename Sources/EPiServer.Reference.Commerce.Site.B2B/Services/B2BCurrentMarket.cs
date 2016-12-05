using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using Mediachase.Commerce;
using Mediachase.Commerce.Core;
using Mediachase.Commerce.Markets;

namespace EPiServer.Reference.Commerce.Site.B2B.Services
{
    public class B2BCurrentMarket : ICurrentMarket
    {
        private const string MarketCookie = "MarketId";
        private static  readonly MarketId DefaultMarketId = new MarketId("US");
        private readonly IMarketService _marketService;
        private readonly CookieService _cookieService;
        private readonly IOrganizationService _organizationService;

        public B2BCurrentMarket(IMarketService marketService, CookieService cookieService, IOrganizationService organizationService)
        {
            _marketService = marketService;
            _cookieService = cookieService;
            _organizationService = organizationService;  
        }

        public IMarket GetCurrentMarket()
        {
            var currentMarket =_cookieService.Get(MarketCookie);
            if (string.IsNullOrEmpty(currentMarket))
            {
                 currentMarket = _organizationService.GetUserCurrentOrganizationLocation();
                if (!string.IsNullOrEmpty(currentMarket))
                    return GetMarket(new MarketId(currentMarket));

                currentMarket = DefaultMarketId.Value;
            }
            return GetMarket(new MarketId(currentMarket));
        }

        public void SetCurrentMarket(MarketId marketId)
        {
            var market = GetMarket(marketId);
            SiteContext.Current.Currency = market.DefaultCurrency;
            _cookieService.Set(MarketCookie, marketId.Value);
        }

        private IMarket GetMarket(MarketId marketId)
        {
            return _marketService.GetMarket(marketId) ?? _marketService.GetMarket(DefaultMarketId);
        }
    }
}