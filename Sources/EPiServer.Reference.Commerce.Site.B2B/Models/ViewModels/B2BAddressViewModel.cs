using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EPiServer.Reference.Commerce.Site.B2B.Models.Contact;

namespace EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels
{
    public class B2BAddressViewModel
    {
        public B2BAddressViewModel(B2BAddress address)
        {
            AddressId = address.AddressId;
            Name = address.Name;
            Street = address.Street;
            City = address.City;
            PostalCode = address.PostalCode;
            CountryCode = address.CountryCode;
            CountryName = address.CountryName;
        }

        public B2BAddressViewModel()
        {
        }

        public Guid AddressId { get; set; }

        [Display(Name = "Address name")]
        [Required(ErrorMessage = "Address name is required")]
        public string Name { get; set; }

        [Display(Name = "Street")]
        [Required(ErrorMessage = "Street is required")]
        public string Street { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Display(Name = "Zip code")]
        [Required(ErrorMessage = "Zip code is required")]
        public string PostalCode { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "Country is required")]
        public string CountryCode { get; set; }
        public string CountryName { get; set; }

        public IEnumerable<B2BCountryViewModel> CountryOptions { get; set; }

        public string AddressString => Street + " " + City + " " + PostalCode + " " + CountryName;
    }
}
