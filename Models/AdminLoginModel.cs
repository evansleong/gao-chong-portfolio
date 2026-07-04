using System.ComponentModel.DataAnnotations;

namespace GaoChongPortfolio.Models
{
    public class AdminLoginModel
    {
        [Required(ErrorMessage = "System authentication requires an administrator ID.")]
        [Display(Name = "Admin Username")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Security verification requires a password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Access Token (Password)")]
        public string Password { get; set; } = string.Empty;
    }
}
