using System;
using System.Collections.Generic;

namespace DataAccessLayer
{
    public class Order
    {
        public int OrderId { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        public OrderStatus Status { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}