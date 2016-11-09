using System;
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
            Line1 = address.Line1;
        }

        public B2BAddressViewModel()
        {
        }

        public Guid AddressId { get; set; }

        [Display(Name = "Address name *:")]
        [Required(ErrorMessage = "Address name is required")]
        public string Name { get; set; }

        [Display(Name = "Address line 1 *:")]
        [Required(ErrorMessage = "Address line 1 is required")]
        public string Line1 { get; set; }
    }
}
