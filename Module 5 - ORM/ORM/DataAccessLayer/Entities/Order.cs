using System;
using System.Collections;
using System.Collections.Generic;

namespace DataAccessLayer.Entities
{
    public class Order
    { 
        public int OrderId { get; set; }

        public string CustomerId { get; set; }

        public Customer Customer { get; set; }
        
        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime ShippedDate { get; set; }

        public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}