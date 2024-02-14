using System.ComponentModel.DataAnnotations;

namespace BlogApp.Entity
{
    public class PostEditViewModel{
        public int PostId { get; set; }
  
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Context { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public string? Image {get;set;}
        public bool changeImage {get;set;}
        public IFormFile? imageFile { get; set; }
         public List<int> tagsId { get; set; } = new List<int>();
        public User User { get; set; } = null!;
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}