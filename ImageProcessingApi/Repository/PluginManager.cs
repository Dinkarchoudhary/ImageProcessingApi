using ImageProcessingApi.IRepository;

namespace ImageProcessingApi.Repository
{
    public class PluginManager : IPluginManager
    {
        private readonly List<IImagePlugin> _plugins;

        public PluginManager()
        {
            _plugins = new List<IImagePlugin>
            {
                new ResizePlugin(),
                new BlurPlugin()
            };
        }

        public IImagePlugin GetPlugin(string name)
        {
            return _plugins.FirstOrDefault(p => p.Name == name);
        }

        public void AddPlugin(IImagePlugin plugin)
        {
            _plugins.Add(plugin);
        }

        public void RemovePlugin(string name)
        {
            var plugin = GetPlugin(name);
            if (plugin != null)
            {
                _plugins.Remove(plugin);
            }
        }

    }
}
