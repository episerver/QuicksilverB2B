using EPiServer.Reference.Commerce.Site.B2B.Models.Entities;

namespace EPiServer.Reference.Commerce.Site.B2B.DomainServiceContracts
{
    public interface IOrganizationDomainService
    {
        B2BOrganization GetCurrentUserOrganizationEntity();
        B2BOrganization GetOrganizationEntityById(string organizationId);
        B2BOrganization GetNewOrganization();
    }
}
