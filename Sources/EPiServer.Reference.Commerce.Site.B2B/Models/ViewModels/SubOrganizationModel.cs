using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using EPiServer.Reference.Commerce.Site.B2B.Models.Entities;

namespace EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels
{
    public class SubOrganizationModel : OrganizationModel
    {
        public SubOrganizationModel(B2BOrganization organization) : base(organization)
        {
            Name = organization.Name;
            Locations = organization.Addresses != null && organization.Addresses.Any()
                ? organization.Addresses.Select(address => new B2BAddressViewModel(address)).ToList()
                : new List<B2BAddressViewModel>();
        }

        public SubOrganizationModel()
        {
            Locations = new List<B2BAddressViewModel>();
        }

        [Display(Name = "Sub-organization name *:")]
        [Required(ErrorMessage = "Sub-organization name is required")]
        public new string Name { get; set; }

        public List<B2BAddressViewModel> Locations { get; set; }
        public IEnumerable<B2BCountryViewModel> CountryOptions { get; set; }
    }
}
