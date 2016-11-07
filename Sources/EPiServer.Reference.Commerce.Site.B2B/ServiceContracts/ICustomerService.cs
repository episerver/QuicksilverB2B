using EPiServer.Reference.Commerce.Site.B2B.Models.Contact;

namespace EPiServer.Reference.Commerce.Site.B2B.ServiceContracts
{
    public interface ICustomerService
    {
        B2BContact GetCurrentContact();
    }
}
