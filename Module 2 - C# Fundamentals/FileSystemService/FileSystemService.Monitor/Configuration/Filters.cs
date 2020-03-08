using System.Configuration;

namespace FileSystemService.Monitor.Configuration
{
    internal sealed class Filters : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new Filter();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as Filter).FilePattern;
        }
    }
}