using ImageProcessingApi.Model;

namespace ImageProcessingApi.IRepository
{
    public interface IImagePlugin
    {
        string Name { get; }
        void ApplyEffect(ref ImageData image, PluginParameters parameters);
    }
}
