using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace DataAccessLayer
{
    public class OrderRepository
    {
        private readonly DbProviderFactory providerFactory;
        private readonly string connectionString;
        
        public OrderRepository(DbProviderFactory providerFactory, string connectionString)
        {
            this.providerFactory = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(providerFactory));
        }
        
        public IEnumerable<Order> GetOrders()
        {
            var orders = new List<Order>();
            var connection = providerFactory.CreateConnection();
            connection.ConnectionString = connectionString;

            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText =
                "SELECT " +
                " [OrderID]" +
                ",[OrderDate]" +
                ",[ShippedDate]" +
                " FROM [Northwind].[dbo].[Orders]";
            command.CommandType = CommandType.Text;

            var dataReader = command.ExecuteReader();
            if (!dataReader.HasRows)
            {
                return null;
            }
            
            while (dataReader.Read())
            {
                var order = MapOrder(dataReader);
                
                orders.Add(order);
            }
            
            dataReader.Close();
            command.Dispose();
            connection.Close();

            return orders;
        }

        public Order GetDetailedOrder(int id)
        {
            var connection = providerFactory.CreateConnection();
            connection.ConnectionString = connectionString;

            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText =
                "SELECT " +
                " [OrderID]" +
                ",[OrderDate]" +
                ",[ShippedDate]" +
                " FROM [Northwind].[dbo].[Orders] AS O" +
                " WHERE O.OrderID = @id;" +

                "SELECT" +
                " P.ProductID," +
                " P.ProductName" +
                " FROM Products AS P" +
                " JOIN [Order Details] AS OD" +
                " ON P.ProductID = OD.ProductID" +
                " JOIN Orders AS O" +
                " ON O.OrderID = @id" +
                " GROUP BY P.ProductID," +
                " P.ProductName;";
            command.CommandType = CommandType.Text;

            var param = command.CreateParameter();
            param.ParameterName = "@id";
            param.Value = id;

            command.Parameters.Add(param);

            var dataReader = command.ExecuteReader();
            if (!dataReader.HasRows)
            {
                return null;
            }

            dataReader.Read();
            var order = MapOrder(dataReader);

            dataReader.NextResult();
            order.Products = new List<Product>();

            while (dataReader.Read())
            {
                order.Products.Add(MapProduct(dataReader));
            }

            dataReader.Close();
            command.Dispose();
            connection.Close();

            return order;
        }

        private Order MapOrder(DbDataReader dataReader)
        {
            var order = new Order();

            order.OrderId = dataReader.GetInt32(0);
            order.OrderDate = (!dataReader.IsDBNull(1)) ? (DateTime?)dataReader.GetDateTime(1) : null;
            order.ShippedDate = (!dataReader.IsDBNull(2)) ? (DateTime?)dataReader.GetDateTime(2) : null;

            if (order.OrderDate == null)
            {
                order.Status = OrderStatus.New;
            }
            else if(order.ShippedDate == null)
            {
                order.Status = OrderStatus.InProgress;
            }
            else
            {
                order.Status = OrderStatus.Shipped;
            }

            return order;
        }


        private Product MapProduct(DbDataReader dataReader)
        {
            var product = new Product();

            product.ProductId = dataReader.GetInt32(0);
            product.ProductName = dataReader.GetString(1);

            return product;
        }
    }
}