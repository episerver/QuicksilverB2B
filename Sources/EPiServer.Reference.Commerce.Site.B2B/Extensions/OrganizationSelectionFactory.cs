using System.Collections.Generic;
using System.Linq;
using EPiServer.Shell.ObjectEditing;
using Mediachase.Commerce.Customers;

namespace EPiServer.Reference.Commerce.Site.B2B.Extensions
{
    public class OrganizationSelectionFactory: ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var organizationsList = CustomerContext.Current.GetOrganizations();

            return organizationsList.Select(organization => new SelectItem()
            {
                Value = organization.Name,
                Text = organization.Name
            }).ToList();
        }
    }
}
