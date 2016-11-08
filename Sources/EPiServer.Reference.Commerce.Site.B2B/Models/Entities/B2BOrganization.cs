using System.Collections.Generic;
using System.Linq;
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

        public string Name { get { return OrganizationEntity.Name; } set { OrganizationEntity.Name = value; } }
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
