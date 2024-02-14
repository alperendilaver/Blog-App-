using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class LoginViewModel{
        [Required]
        [EmailAddress]
        [Display(Name ="E-Posta")]
        public string? Email { get; set; }
        [Required]
        [StringLength(15,ErrorMessage ="Maksimum 15 karakter",MinimumLength =1)]
        [DataType(DataType.Password)]
        [Display(Name ="Parola")]
        public string? Password { get; set; }
    }
}