using System.Configuration;

namespace FileSystemService.Monitor.Configuration
{
    internal sealed class MonitorSection : ConfigurationSection
    {
        [ConfigurationProperty("appName")]
        public string ApplicationName => (string) base["appName"];
        
        [ConfigurationCollection(typeof(WatchDirectory), 
            AddItemName = "directory", 
            ClearItemsName = "clear", 
            RemoveItemName = "delete")]
        [ConfigurationProperty("directories")]
        public WatchDirectories WatchDirectories => (WatchDirectories) this["directories"];

        [ConfigurationCollection(typeof(Filter), 
            AddItemName = "filter", 
            ClearItemsName = "clear", 
            RemoveItemName = "delete")]
        [ConfigurationProperty("filters")]
        public Filters Filters => (Filters) this["filters"];
    }
}