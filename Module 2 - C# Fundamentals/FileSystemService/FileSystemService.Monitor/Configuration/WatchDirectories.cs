using System.Configuration;

namespace FileSystemService.Monitor.Configuration
{
    internal sealed class WatchDirectories : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new WatchDirectory();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as WatchDirectory).Path;
        }
    }
}