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
                ",[CustomerID]" +
                ",[EmployeeID]" +
                ",[OrderDate]" +
                ",[RequiredDate]" +
                ",[ShippedDate]" +
                ",[ShipVia]" +
                ",[Freight]" +
                ",[ShipName]" +
                ",[ShipAddress]" +
                ",[ShipCity]" +
                ",[ShipRegion]" +
                ",[ShipPostalCode]" +
                ",[ShipCountry]" +
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
                ",[CustomerID]" +
                ",[EmployeeID]" +
                ",[OrderDate]" +
                ",[RequiredDate]" +
                ",[ShippedDate]" +
                ",[ShipVia]" +
                ",[Freight]" +
                ",[ShipName]" +
                ",[ShipAddress]" +
                ",[ShipCity]" +
                ",[ShipRegion]" +
                ",[ShipPostalCode]" +
                ",[ShipCountry]" +
                " FROM [Northwind].[dbo].[Orders] AS O" +
                " WHERE O.OrderID = @id;" +

                "select" +
                " P.ProductID," +
                " P.ProductName" +
                " from Products as P" +
                " join[Order Details] AS OD" +
                " on P.ProductID = OD.ProductID" +
                " join Orders AS O" +
                " on O.OrderID = @id" +
                " group by P.ProductID," +
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
            order.CustomerId = (!dataReader.IsDBNull(1)) ? dataReader.GetString(1) : null;
            order.EmployeeId = (!dataReader.IsDBNull(2)) ? (int?)dataReader.GetInt32(2) : null;
            order.OrderDate = (!dataReader.IsDBNull(3)) ? (DateTime?)dataReader.GetDateTime(3) : null;
            order.RequiredDate = (!dataReader.IsDBNull(4)) ? (DateTime?)dataReader.GetDateTime(4) : null;
            order.ShippedDate = (!dataReader.IsDBNull(5)) ? (DateTime?)dataReader.GetDateTime(5) : null;
            order.ShipVia = (!dataReader.IsDBNull(6)) ? (int?)dataReader.GetInt32(6) : null;
            order.Freight = (!dataReader.IsDBNull(7)) ? (int?)dataReader.GetDecimal(7) : null;
            order.ShipName = (!dataReader.IsDBNull(8)) ? dataReader.GetString(8): null;
            order.ShipAddress = (!dataReader.IsDBNull(9)) ? dataReader.GetString(9) : null;
            order.ShipCity = (!dataReader.IsDBNull(10)) ? dataReader.GetString(10) : null;
            order.ShipRegion = (!dataReader.IsDBNull(11)) ? dataReader.GetString(11) : null;
            order.ShipPostalCode = (!dataReader.IsDBNull(12)) ? dataReader.GetString(12) : null;
            order.ShipCountry = (!dataReader.IsDBNull(13)) ? dataReader.GetString(13) : null;

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