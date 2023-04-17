namespace BookStore.Util
{
    public class ImageUtil
    {

        public static string SaveImage(IFormFile file,string subfolder, IWebHostEnvironment _env)
        {
            // Get the file name and extension
            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string extension = Path.GetExtension(file.FileName);

            // Generate a unique file name
            string uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

            // Get the path to the folder where the image will be saved
            string uploadsFolder = Path.Combine(_env.WebRootPath, $"image/{subfolder}");

            // Create the folder if it doesn't exist
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Get the path to the file where the image will be saved
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the image to the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return uniqueFileName;
        }
    }
}
