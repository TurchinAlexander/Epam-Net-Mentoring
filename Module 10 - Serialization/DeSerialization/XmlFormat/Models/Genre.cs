using System.Xml.Serialization;

namespace XmlFormat.Models
{
    public enum Genre
    {
        Computer,
        Fantasy,
        Romance,
        Horror,
        [XmlEnum(Name = "Science Fiction")]
        ScienceFiction,
    }
}