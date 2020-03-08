using System.Configuration;

namespace FileSystemService.Monitor.Configuration
{
    internal sealed class Filter : ConfigurationElement
    {
        [ConfigurationProperty("pattern", IsKey = true, IsRequired = true)]
        public string FilePattern => (string) this["pattern"];

        //[RegexStringValidator(@"(\w):(\\(\w)+)+")]
        [ConfigurationProperty("destinationFolder", IsRequired = true)]
        public string DestinationFolder => (string) this["destinationFolder"];
        
        [ConfigurationProperty("addPositionNumber", DefaultValue = false)]
        public bool ShouldAddPositionNumber => (bool) this["addPositionNumber"];

        [ConfigurationProperty("addMovementDate", DefaultValue = false)]
        public bool ShouldAddMovementDate => (bool) this["addMovementDate"];
    }
}