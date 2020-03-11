using System.Configuration;

namespace FileSystemService.ConsoleProject.Configuration
{
    public class ConsoleSection : ConfigurationSection
    {
        [ConfigurationProperty("cultureLocalization")]
        public CultureLocalization CultureLocalization => (CultureLocalization) this["cultureLocalization"];
    }
}