using System.Configuration;

namespace FileSystemService.Monitor.Configuration
{
    internal sealed class WatchDirectory : ConfigurationElement
    {
        [ConfigurationProperty("path", IsKey = true, IsRequired = true)]
        public string Path => (string)this["path"];
    }
}