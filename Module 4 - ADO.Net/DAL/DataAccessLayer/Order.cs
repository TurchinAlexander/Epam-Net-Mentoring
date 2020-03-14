using System;
using System.Collections.Generic;

namespace DataAccessLayer
{
    public class Order
    {
        private int orderId;

        public int OrderId
        {
            get => orderId;
            set
            {
                CanChange();

                orderId = value;
            }
        }

        public DateTime? OrderDate { get; internal set; }

        public DateTime? ShippedDate { get; internal set; }

        public OrderStatus Status { get; internal set; }

        public ICollection<Product> Products { get; set; }

        private void CanChange()
        {
            if (Status != OrderStatus.New)
            {
                throw new InvalidOperationException();
            }
        }
    }
}