using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ContactsContext _context;

        public ContactsController(ContactsContext context)
        {
            _context = context;
        }

        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactDisplay>>> GetContacts()
        {
            return await _context.Contacts.Include("PhoneNumbers").Select(cd => ContactDisplay.CreateContactDisplay(cd))
                .ToListAsync();
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactDisplay>> GetContact(Guid id)
        {
            var contact = await _context.Contacts.Include("PhoneNumbers").FirstOrDefaultAsync(n => n.ContactId == id);

            if (contact == null)
            {
                return NotFound();
            }

            return ContactDisplay.CreateContactDisplay(contact);
        }

        // PUT: api/Contacts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(Guid id, ContactDisplay contact)
        {
            if (id != contact.ContactId)
            {
                return BadRequest();
            }

            var contactEntity =
                await _context.Contacts.Include("PhoneNumbers").FirstOrDefaultAsync(i => i.ContactId == id);

            if (contactEntity == null)
            {
                return NotFound();
            }

            contactEntity.UpdateContactFromDisplay(contact);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when(!ContactExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Contacts
        [HttpPost]
        public async Task<ActionResult<Contact>> PostContact(ContactDisplay contact)
        {
            var contactEntity = new Contact
            {
                Address = contact.Address,
                Name = contact.Name,
                DateOfBirth = contact.DateOfBirth,
                PhoneNumbers = contact.Numbers.Select(n => new PhoneNumber
                {
                    Number = n.Number
                }).ToList()
            };

            _context.Contacts.Add(contactEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetContact), new {id = contact.ContactId}, ContactDisplay.CreateContactDisplay(contactEntity));
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Contact>> DeleteContact(Guid id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContactExists(Guid id)
        {
            return _context.Contacts.Any(e => e.ContactId == id);
        }
    }
}
