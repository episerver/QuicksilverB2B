using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.ServiceLocation;

namespace EPiServer.Reference.Commerce.Site.B2B.Services
{
    [ServiceConfiguration(typeof(IQuickOrderService), Lifecycle = ServiceInstanceScope.Singleton)]
    public class QuickOrderService : IQuickOrderService
    {
    }
}
