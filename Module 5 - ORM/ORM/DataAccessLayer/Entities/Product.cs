using System.Collections.Generic;

namespace DataAccessLayer.Entities
{
    public class Product
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }
        
        public int QuantityPerUnit { get; set; }

        public decimal UnitPrice { get; set; }

        public int UnitsInStock { get; set; }

        public int UnitsOnOrder { get; set; }

        public int ReorderLevel { get; set; }

        public bool Dicontinued { get; set; }

        public int CategoryId { get; set; }
        
        public Category Category { get; set; }

        public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}