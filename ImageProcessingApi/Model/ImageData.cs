namespace ImageProcessingApi.Model
{
    public class ImageData
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; } = "Original Image";

        public static ImageData FromFormFile(int id, string uploadFolder, IFormFile file)
        {
            var filePath = Path.Combine(uploadFolder, $"{id}_{file.FileName}");
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return new ImageData { Id = id, FilePath = filePath };
        }
    }
}
