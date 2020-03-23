using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactsApi.Models
{
    public class PhoneNumber
    {
        public Guid PhoneNumberId { get; set; }
        [Required]
        public string Number { get; set; }
        /// <summary>
        /// Foreign key to Contact
        /// </summary>
        public Guid ContactId { get; set; }
        /// <summary>
        /// Navigation
        /// </summary>
        [ForeignKey("ContactId")]
        public Contact Contact { get; set; }

        public void UpdateNumberFormDisplay(PhoneNumberDisplay phoneNumberDisplay)
        {
            if (phoneNumberDisplay == null)
                return;
            this.Number = phoneNumberDisplay.Number;
        }
    }

    public class PhoneNumberDisplay
    {
        public static PhoneNumberDisplay CreatePhoneNumberDisplay(PhoneNumber phoneNumber)
        {
            return new PhoneNumberDisplay
            {
                PhoneNumberId = phoneNumber.PhoneNumberId,
                Number = phoneNumber.Number,
                ContactId = phoneNumber.ContactId,
            };
        }
        public Guid PhoneNumberId { get; set; }
        
        public string Number { get; set; }
       
        public Guid ContactId { get; set; }
    }
}
