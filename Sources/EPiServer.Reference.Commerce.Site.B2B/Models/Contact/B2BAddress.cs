using System;
using Mediachase.BusinessFoundation.Data;
using Mediachase.BusinessFoundation.Data.Business;
using Mediachase.Commerce.Customers;

namespace EPiServer.Reference.Commerce.Site.B2B.Models.Contact
{
    public class B2BAddress
    {
        public B2BAddress(CustomerAddress customerAddress)
        {
            Address = customerAddress;
        }
        public CustomerAddress Address { get; set; }
        public Guid AddressId
        {
            get { return Address.AddressId; }
            set { Address.AddressId = (PrimaryKeyId)value; }
        }
        public string Name { get { return Address.Name; } set { Address.Name = value; } }
        public string Line1 { get { return Address.Line1; } set { Address.Line1 = value; } }
        public Guid OrganizationId {
            get { return Address.OrganizationId ?? Guid.Empty; }
            set { Address.OrganizationId = (PrimaryKeyId?) value; }
        }
        public void SaveChanges()
        {
            BusinessManager.Update(Address);
        }
    }
}
