using System;
using System.Collections.Generic;
using DataAccessLayer.Attributes;

namespace DataAccessLayer.OrderSection.Models
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

        public DateTime? RequiredDate { get; internal set; }

        public DateTime? ShippedDate { get; internal set; }

        public ICollection<Product> Products { get; set; }

        public OrderStatus GetStatus()
        {
            if (OrderDate == null)
            {
                return OrderStatus.New;
            }

            if (ShippedDate == null)
            {
                return OrderStatus.InProgress;
            }

            return OrderStatus.Shipped;
            
        }

        private void CanChange()
        {
            if (GetStatus() != OrderStatus.New)
            {
                throw new InvalidOperationException();
            }
        }
    }
}