using System;
using System.Security.Permissions;
using System.Xml.Serialization;

namespace XmlFormat.Models
{
    public class Book
    {
        [XmlAttribute("id")]
        public string BookId { get; set; }

        [XmlElement("isbn")]
        public string Isbn { get; set; }

        [XmlElement("author")]
        public string Author { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("genre")]
        public Genre Genre { get; set; }

        [XmlElement("publisher")]
        public string Publisher { get; set; }

        [XmlElement("publish_date", DataType = "date")]
        public DateTime PublisherDate { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("registration_date", DataType = "date")]
        public DateTime RegistrationDate { get; set; }
    }
}