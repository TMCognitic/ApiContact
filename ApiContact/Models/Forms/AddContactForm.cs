using System.ComponentModel.DataAnnotations;

namespace ApiContact.Models.Forms
{
#nullable disable
    public class AddContactForm
    {
        [Required]
        [MinLength(1)]
        public string LastName { get; set; }
        [Required]
        [MinLength(1)]
        public string FirstName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
    }
#nullable enable
}
