using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;

namespace BlogApp.Models{
    public class PostCreateViewModel
    {

        public int PostID { get; set; }

        [Required]
        [Display(Name = "Başlık")]
        public string? title { get; set; }

        [Required]
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }


        [Required]
        [Display(Name = "İçerik")]
        public string? Content { get; set; }

        [Required]
        [Display(Name = "Url")]
        public string? Url { get; set; }

        public bool IsActive { get; set; }

        public List<Tag> Tags { get; set; } = new();
    }
}