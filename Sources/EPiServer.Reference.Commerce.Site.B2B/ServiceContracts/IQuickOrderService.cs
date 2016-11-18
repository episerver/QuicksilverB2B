using EPiServer.Core;

namespace EPiServer.Reference.Commerce.Site.B2B.ServiceContracts
{
    public interface IQuickOrderService
    {
        string ValidateProduct(ContentReference variationReference, decimal quantity, string code);
    }
}
