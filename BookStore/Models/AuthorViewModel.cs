using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using BookStore.Validators;

namespace BookStore.Models
{
    public class AuthorViewModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Author Name")]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "The name must contain only letters, numbers, and spaces.")]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; }

        [Required]
        [StringLength(500)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Author picture")]
        [ImageValidator(1024*1024*5)] // 5 MB
        public IFormFile AuthorPicture { get; set; }
    }
}
