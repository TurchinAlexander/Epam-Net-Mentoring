using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using XmlFormat.Models;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Catalog));

            FileStream stream = new FileStream("books.xml", FileMode.Open);
            Catalog catalog = xmlSerializer.Deserialize(stream) as Catalog;

            Console.WriteLine("Good");
        }
    }
}
