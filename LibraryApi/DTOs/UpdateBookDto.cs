using System.ComponentModel.DataAnnotations;

namespace LibraryApi.DTOs
{
    public class UpdateBookDto
    {
        [Required(ErrorMessage = "title is required")]
        [StringLength(200, ErrorMessage = "title can be at most 200 characters")]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "author is required")]
        [StringLength(100, ErrorMessage = "author can be at most 100 characters")]
        public string Author { get; set; } = "";

        [Range(1, 2100, ErrorMessage = "year must be between 1 and 2100")]
        public int Year { get; set; }
    }
}
