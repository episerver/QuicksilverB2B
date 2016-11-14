using System.Collections.Generic;
using EPiServer.Reference.Commerce.Site.B2B.Models.Entities;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;

namespace EPiServer.Reference.Commerce.Site.B2B.ServiceContracts
{
    public interface IAddressService
    {
        void UpdateOrganizationAddress(B2BOrganization organization, B2BAddressViewModel addressModel);
        IEnumerable<B2BCountryViewModel> GetAllCountries();
        string GetCountryNameByCode(string code);
        void DeleteAddress(string organizationId, string addressId);
    }
}
