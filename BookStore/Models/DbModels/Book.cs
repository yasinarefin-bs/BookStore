using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.DbModels
{
    public class Book
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
        [Display(Name = "Cover picture")]
        public string CoverPicUrl { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public Author Author { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
