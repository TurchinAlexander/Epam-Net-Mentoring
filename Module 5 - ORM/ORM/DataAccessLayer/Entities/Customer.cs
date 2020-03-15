using System;

namespace DataAccessLayer.Entities
{
    public class Customer
    {
        public string CustomerId { get; set; }

        public string CompanyName { get; set; }

        public string ContactName { get; set; }

        public string ContactTitle { get; set; }

        public DateTime FoundationDate { get; set; }

        public string Address { get; set; }
    }
}