using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class RegisterViewModel{
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        [Compare(nameof(Password),ErrorMessage ="parolanız eşleşmiyor")]
        public string? ConfirmPassword { get; set; }
        
        [Required]
        [Display(Name ="Fotoğraf")]
        public IFormFile? imageFile {get;set;}
        
        public string? Image { get; set; }
    }
}