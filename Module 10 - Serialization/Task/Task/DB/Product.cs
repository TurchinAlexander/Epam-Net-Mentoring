using System.Collections;
using System.Linq;
using System.Runtime.Serialization;

namespace Task.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Serializable]
    public partial class Product : ISerializable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Order_Details = new HashSet<Order_Detail>();
        }

        public int ProductID { get; set; }

        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }

        public int? SupplierID { get; set; }

        public int? CategoryID { get; set; }

        [StringLength(20)]
        public string QuantityPerUnit { get; set; }

        [Column(TypeName = "money")]
        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Detail> Order_Details { get; set; }

        public virtual Supplier Supplier { get; set; }
        
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            using (var dbContext = new Northwind())
            {
                dbContext.Configuration.ProxyCreationEnabled = false;

                if (CategoryID.HasValue)
                {
                    var category = dbContext.Categories
                        .First(c => c.CategoryID == CategoryID);

                    info.AddValue("CategoryID", CategoryID);
                    info.AddValue("Category", Category);
                }

                if (SupplierID.HasValue)
                {
                    var supplier = dbContext.Suppliers
                        .First(s => s.SupplierID == SupplierID);

                    info.AddValue("SupplierID", SupplierID);
                    info.AddValue("Supplier", supplier);
                }
                
                info.AddValue("ProductID", ProductID);
                info.AddValue("ProductName", ProductName);
                info.AddValue("QuantityPerUnit", QuantityPerUnit);
                info.AddValue("UnitPrice", UnitPrice);
                info.AddValue("UnitsInStock", UnitsInStock);
                info.AddValue("UnitsOnOrder", UnitsOnOrder);
                info.AddValue("ReorderLevel", ReorderLevel);
                info.AddValue("Discontinued", Discontinued);

                var orderDetails = dbContext.Order_Details
                    .Where(o => o.ProductID == ProductID)
                    .ToArray();

                info.AddValue("Order_Details", orderDetails);
            }

            
        }

        public Product(SerializationInfo info, StreamingContext context)
        {
            ProductID = info.GetInt32("ProductID");
            ProductName = info.GetString("ProductName");
            QuantityPerUnit = info.GetString("QuantityPerUnit");
            UnitPrice = (decimal?)info.GetValue("UnitPrice", typeof(decimal?));
            UnitsInStock = (short?)info.GetValue("UnitsInStock", typeof(short?));
            UnitsOnOrder = (short?)info.GetValue("UnitsOnOrder", typeof(short?));
            ReorderLevel = (short?)info.GetValue("ReorderLevel", typeof(short?));
            Discontinued = info.GetBoolean("Discontinued");

            Order_Details = (ICollection<Order_Detail>)info.GetValue("Order_Details", typeof(ICollection<Order_Detail>));
            foreach (var orderDetail in Order_Details)
            {
                orderDetail.Product = this;
            }

            CategoryID = (int?)info.GetValue("CategoryID", typeof(int?));
            if (CategoryID.HasValue)
            {
                Category = (Category)info.GetValue("Category", typeof(Category));
            }

            SupplierID = (int?)info.GetValue("SupplierID", typeof(int?));
            if (SupplierID.HasValue)
            {
                Supplier = (Supplier)info.GetValue("Supplier", typeof(Supplier));
            }
        }
    }
}
