using System;
using System.Collections.Generic;
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
            if (organization.SubOrganizations.Count > 0)
            {
                SubOrganizations = new List<OrganizationModel>();
                foreach (var c in organization.SubOrganizations)
                {
                    SubOrganizations.Add(new OrganizationModel(c));
                }
            }
            ParentOrganizationId = organization.ParentOrganizationId;
        }

        public OrganizationModel()
        {
        }

        public Guid OrganizationId { get; set; }
        
        [Display(Name = "Organization name *:")]
        [Required(ErrorMessage = "Organization Name is required")]
        public string Name { get; set; }

        public B2BAddressViewModel Address { get; set; }
        public List<OrganizationModel> SubOrganizations { get; set; }
        public Guid ParentOrganizationId { get; set; }
        public OrganizationModel ParentOrganization { get; set; }
    }
}
