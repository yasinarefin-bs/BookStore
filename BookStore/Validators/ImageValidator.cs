using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace BookStore.Validators
{




    public class ImageValidator : ValidationAttribute
    {

        private readonly int _maxFileSize;
        private readonly string[] _allowedExtensions = { ".jpeg", ".jpg", ".png", ".gif", ".webp" };

        public ImageValidator(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = System.IO.Path.GetExtension(file.FileName).ToLowerInvariant();

                if(file == null)
                {
                    return new ValidationResult($"Image cannot be null");
                }

                if (!string.IsNullOrEmpty(extension) && !_allowedExtensions.Contains(extension))
                {
                    return new ValidationResult($"Only files with extensions: {string.Join(", ", _allowedExtensions)} are allowed.");
                }

                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult($"The file size cannot exceed {_maxFileSize / 1024} KB.");
                }

                try
                {
                    using (var image = Image.FromStream(file.OpenReadStream()))
                    {
                        // The file is an image
                    }
                }
                catch
                {
                    return new ValidationResult($"Only image files are allowed.");
                }
            }
            else
            {
                return new ValidationResult($"Iform files only");
            }

            return ValidationResult.Success;
        }

        
    }

}
