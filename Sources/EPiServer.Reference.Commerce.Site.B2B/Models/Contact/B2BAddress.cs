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
        public string Street { get { return Address.Line1; } set { Address.Line1 = value; } }
        public string City { get { return Address.City; } set { Address.City = value; } }
        public string PostalCode { get { return Address.PostalCode; } set { Address.PostalCode = value; } }
        public string CountryCode { get { return Address.CountryCode; } set { Address.CountryCode = value; } }
        public string CountryName { get { return Address.CountryName; } set { Address.CountryName = value; } }

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
