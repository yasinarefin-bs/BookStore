using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using BookStore.Validators;

namespace BookStore.Models
{
    public class BookViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(60)]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "The name must contain only letters, numbers, and spaces.")]
        [Display(Name = "Book Title")]

        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Language { get; set; }

        [Required]
        [ImageValidator(1024 * 1024 * 5)] // 5 MB
        public IFormFile CoverPic { get; set; }

        [Required]
        public int AuthorId { get; set; }
    }
}
