using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ContactsApi.Models
{
    public class Contact
    {
        public Guid ContactId { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Address { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }
        public ICollection<PhoneNumber> PhoneNumbers { get; set; }

        public void UpdateContactFromDisplay(ContactDisplay contactDisplay)
        {
            this.Name = contactDisplay.Name;
            this.Address = contactDisplay.Address;
            this.DateOfBirth = contactDisplay.DateOfBirth;

            foreach (var numberDisplay in contactDisplay.Numbers)
            {
                if (PhoneNumbers.Any(p => p.PhoneNumberId == numberDisplay.PhoneNumberId))
                {
                    var phoneNumber = PhoneNumbers.Single(p => p.PhoneNumberId == numberDisplay.PhoneNumberId);
                    phoneNumber.UpdateNumberFormDisplay(numberDisplay);
                }
                else
                {
                    PhoneNumbers.Add(new PhoneNumber
                    {
                        ContactId = numberDisplay.ContactId,
                        Number = numberDisplay.Number
                    });
                }
            }

            foreach (var number in PhoneNumbers)
            {
                if (contactDisplay.Numbers.All(p => p.PhoneNumberId != number.PhoneNumberId))
                    PhoneNumbers.Remove(number);
            }
        }
        
    }

    public class ContactDisplay
    {
        public Guid ContactId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<PhoneNumberDisplay> Numbers { get; set; }

        public static ContactDisplay CreateContactDisplay(Contact contact)
        {
            return new ContactDisplay
            {
                ContactId = contact.ContactId,
                Name = contact.Name,
                Address = contact.Address,
                DateOfBirth = contact.DateOfBirth,
                Numbers = contact.PhoneNumbers.Select(PhoneNumberDisplay.CreatePhoneNumberDisplay)
                    .ToList()

            };
        }
    }
}
