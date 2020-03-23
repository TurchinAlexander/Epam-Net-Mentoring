using System.Linq;
using System.Runtime.Serialization;
using Task.DB;

namespace Task
{
    public class OrderDetailsSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var orderDetail = (Order_Detail)obj;

            using (var dbContext = new Northwind())
            {
                dbContext.Configuration.ProxyCreationEnabled = false;

                var order = dbContext.Orders
                    .First(o => o.OrderID == orderDetail.OrderID);

                var product = dbContext.Products
                    .First(p => p.ProductID == orderDetail.ProductID);

                info.AddValue("OrderID", orderDetail.OrderID);
                info.AddValue("Order", order);
                info.AddValue("ProductID", orderDetail.ProductID);
                info.AddValue("Product", product);
                info.AddValue("UnitPrice", orderDetail.UnitPrice);
                info.AddValue("Quantity", orderDetail.Quantity);
                info.AddValue("Discount", orderDetail.Discount);

            }

        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var orderDetail = (Order_Detail)obj;

            orderDetail.ProductID = (int) info.GetValue("ProductID", typeof(int));
            orderDetail.Product = (Product) info.GetValue("Product", typeof(Product));
            orderDetail.OrderID = (int) info.GetValue("OrderID", typeof(int));
            orderDetail.Order = (Order) info.GetValue("Order", typeof(Order));
            orderDetail.UnitPrice = (decimal) info.GetValue("UnitPrice", typeof(decimal));
            orderDetail.Quantity = (short) info.GetValue("Quantity", typeof(short));
            orderDetail.Discount = (float) info.GetValue("Discount", typeof(float));

            return orderDetail;
        }
    }
}