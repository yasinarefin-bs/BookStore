using BookStore.Controllers;

namespace BookStore.Util
{
    public class ImageUtil
    {

        public static string SaveImage(IFormFile file,string subfolder, IWebHostEnvironment _env)
        {
            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string extension = Path.GetExtension(file.FileName);

            string uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

            string uploadsFolder = Path.Combine(_env.WebRootPath, $"image/{subfolder}");

            // Create the folder if it doesn't exist
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return uniqueFileName;
        }


        public static bool DeleteImage(string filePath, IWebHostEnvironment _env, ILogger<BookController> logger)
        {
            // Get the path to the folder where the image is saved
            string absolutePath = Path.Combine(_env.WebRootPath,filePath);


            logger.LogInformation(absolutePath);
            logger.LogInformation("Webroot = " + _env.WebRootPath);


            // Check if the file exists
            if (File.Exists(absolutePath))
            {

                logger.LogInformation("Exists");
                // Delete the file
                File.Delete(absolutePath);
                return true;
            }
            return false;
        }
    }
}
