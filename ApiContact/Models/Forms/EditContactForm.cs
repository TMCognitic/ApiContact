using System.ComponentModel.DataAnnotations;

namespace ApiContact.Models.Forms
{
#nullable disable
    public class EditContactForm
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
    }
#nullable disable
}
