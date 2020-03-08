using System;
using System.Configuration;
using System.Globalization;
using System.Linq;

namespace FileSystemService.ConsoleProject.Configuration
{
    public class CultureLocalization : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "en-Us", IsRequired = true)]
        public string Name => (string) this["name"];
    }
    

}