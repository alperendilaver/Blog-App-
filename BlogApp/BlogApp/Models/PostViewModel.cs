using System.ComponentModel.DataAnnotations;
using BlogApp.Entity;

namespace BlogApp.Models
{
    public class PostViewModel{
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Context { get; set; }

        public bool IsActive { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public IFormFile? imageFile { get; set; }
          public string? image { get; set; }
        
        [Required]
        public List<int> tagsId { get; set; } = new List<int>();
    }
}