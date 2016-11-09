using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Reference.Commerce.Site.B2B.Models.Contact;
using Mediachase.BusinessFoundation.Data;
using Mediachase.Commerce.Customers;

namespace EPiServer.Reference.Commerce.Site.B2B.Models.Entities
{
    public class B2BOrganization
    {
        public B2BOrganization(Organization organization)
        {
            OrganizationEntity = organization;
        }

        public Organization OrganizationEntity { get; set; }

        public Guid OrganizationId
        {
            get { return OrganizationEntity.PrimaryKeyId ?? Guid.Empty; }
            set { OrganizationEntity.PrimaryKeyId = (PrimaryKeyId?)value; }
        }

        public string Name { get { return OrganizationEntity.Name; } set { OrganizationEntity.Name = value; } }

        public B2BAddress Address => OrganizationEntity.Addresses != null && OrganizationEntity.Addresses.Any()
            ? new B2BAddress(OrganizationEntity.Addresses.FirstOrDefault())
            : null;

        public List<B2BOrganization> SubOrganizations
        {
            get
            {
                return
                    OrganizationEntity.ChildOrganizations.Select(
                        childOrganization => new B2BOrganization(childOrganization)).ToList();
            }
        }

        public void SaveChanges()
        {
            OrganizationEntity.SaveChanges();
        }
    }
}
