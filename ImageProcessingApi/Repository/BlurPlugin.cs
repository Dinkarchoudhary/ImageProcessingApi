using ImageProcessingApi.IRepository;
using ImageProcessingApi.Model;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImageProcessingApi.Repository
{
    public class BlurPlugin : IImagePlugin
    {
        public string Name => "Blur";

        public void ApplyEffect(ref ImageData image, PluginParameters parameters)
        {
            // Load the image from the file
            using (var img = Image.Load(image.FilePath))
            {
                // Apply the blur effect
                img.Mutate(x => x.GaussianBlur(parameters.Radius));

                // Save the modified image
                var outputFilePath = GetOutputFilePath(image.FilePath, "blurred");
                img.Save(outputFilePath);

                // Update the image data
                image.FilePath = outputFilePath;
                image.Description += $" Blurred with radius {parameters.Radius}.";
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
