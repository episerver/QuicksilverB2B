using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Reference.Commerce.Site.B2B.Models.Entities;

namespace EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels
{
    public class OrganizationModel
    {
        public OrganizationModel(B2BOrganization organization)
        {
            OrganizationId = organization.OrganizationId;
            Name = organization.Name;
            Address = organization.Address != null ? new B2BAddressViewModel(organization.Address) : null;
        }

        public OrganizationModel()
        {
        }

        public Guid OrganizationId { get; set; }

        [Display(Name = "Organization name *:")]
        [Required(ErrorMessage = "Organization Name is required")]
        public string Name { get; set; }

        public B2BAddressViewModel Address { get; set; }
    }
}
