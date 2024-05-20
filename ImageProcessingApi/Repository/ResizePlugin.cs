using ImageProcessingApi.IRepository;
using ImageProcessingApi.Model;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImageProcessingApi.Repository
{
    public class ResizePlugin : IImagePlugin
    {
        public string Name => "Resize";

        public void ApplyEffect(ref ImageData image, PluginParameters parameters)
        {
            // Load the image from the file
            using (var img = Image.Load(image.FilePath))
            {
                // Apply the resize effect
                img.Mutate(x => x.Resize(parameters.Size, parameters.Size));

                // Save the modified image
                var outputFilePath = GetOutputFilePath(image.FilePath, "resized");
                img.Save(outputFilePath);

                // Update the image data
                image.FilePath = outputFilePath;
                image.Description += $" Resized to {parameters.Size}x{parameters.Size} pixels.";
            }
        }

        private string GetOutputFilePath(string originalFilePath, string suffix)
        {
            var directory = Path.GetDirectoryName(originalFilePath);
            var filename = Path.GetFileNameWithoutExtension(originalFilePath);
            var extension = Path.GetExtension(originalFilePath);
            return Path.Combine(directory, $"{filename}_{suffix}{extension}");
        }


    }
}
