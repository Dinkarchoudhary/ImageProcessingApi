namespace ImageProcessingApi.IRepository
{
    public interface IPluginManager
    {
        IImagePlugin GetPlugin(string name);
        void AddPlugin(IImagePlugin plugin);
        void RemovePlugin(string name);
    }
}
