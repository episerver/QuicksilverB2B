using EPiServer.Find.ClientConventions;
using EPiServer.Find.Commerce;
using EPiServer.Reference.Commerce.Site.Features.Product.Models;
using EPiServer.ServiceLocation;

namespace EPiServer.Reference.Commerce.Site.Infrastructure.Business
{
    [ServiceConfiguration(ServiceType = typeof(CatalogContentClientConventions), Lifecycle = ServiceInstanceScope.Singleton)]
    public class B2BCatalogContentClientConventions : CatalogContentClientConventions
    {
        protected override void ApplyNestedConventions(NestedConventions nestedConventions)
        {
            base.ApplyNestedConventions(nestedConventions);
            nestedConventions.ForType<BaseProduct>().Add(p => p.ListingPrices);
            nestedConventions.ForType<BaseProduct>().Add(p => p.OriginalPrices);
        }
    }
}