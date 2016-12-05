using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;

namespace EPiServer.Reference.Commerce.Site.B2B.ServiceContracts
{
    public interface IOrganizationService
    {
        OrganizationModel GetCurrentUserOrganization();
        void CreateOrganization(OrganizationModel organizationInfo);
        void UpdateOrganization(OrganizationModel organizationInfo);
        void CreateSubOrganization(SubOrganizationModel newSubOrganization);
        SubOrganizationModel GetSubOrganizationById(string subOrganizationId);
        void UpdateSubOrganization(SubOrganizationModel subOrganizationModel);
        string GetUserCurrentOrganizationLocation();
    }
}
