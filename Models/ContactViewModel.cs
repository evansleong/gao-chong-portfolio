using System.ComponentModel.DataAnnotations;

namespace GaoChongPortfolio.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Even compilers need a source name. Please provide your name.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters (we support long names, but buffer overflows are bad).")]
        [Display(Name = "Your Name / Identifier")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Where should I routing responses? Email is required.")]
        [EmailAddress(ErrorMessage = "This email failed regex validation. Please provide a valid address.")]
        [Display(Name = "IP Endpoint (Email)")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A blank request payload is redundant. Please type a message.")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "A message must contain at least 10 characters to bypass my local spam logic.")]
        [Display(Name = "Payload (Message)")]
        public string Message { get; set; } = string.Empty;

        // Anti-spam honeypot field. If this is filled, we classify the sender as a malicious bot.
        [Display(Name = "Bait Field (Leave empty)")]
        public string? Honeypot { get; set; }
    }
}
