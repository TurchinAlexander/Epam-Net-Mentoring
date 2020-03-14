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

        public Order Add(Order order)
        {
            var orders = new List<Order>();
            var connection = providerFactory.CreateConnection();
            connection.ConnectionString = connectionString;

            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = "insert into orders (requireddate) values (getdate());" +
                                  "select @@IDENTITY;";

            order.OrderId = Convert.ToInt32(command.ExecuteScalar());
            order.RequiredDate = DateTime.Now;

            command.Dispose();
            connection.Close();

            return order;
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
                ",[RequiredDate]" +
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
                ",[RequiredDate]" +
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

        public Order Delete(int id)
        {
            var order = GetDetailedOrder(id);

            if (order.Status == OrderStatus.Shipped)
            {
                return order;
            }

            var orders = new List<Order>();
            var connection = providerFactory.CreateConnection();
            connection.ConnectionString = connectionString;

            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = "delete orders where orderID = @id";

            var param = command.CreateParameter();
            param.ParameterName = "@id";
            param.Value = id;

            command.Parameters.Add(param);

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Close();

            return order;
        }

        public Order SetOrderedDate(Order order)
        {
            var connection = providerFactory.CreateConnection();
            connection.ConnectionString = connectionString;

            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = "update orders set orderdate = getdate() where orderid = @id ";

            var param = command.CreateParameter();
            param.ParameterName = "@id";
            param.Value = order.OrderId;

            command.Parameters.Add(param);

            command.ExecuteNonQuery();

            order.OrderDate = DateTime.Now;
            order.Status = OrderStatus.InProgress;

            command.Dispose();
            connection.Close();

            return order;
        }

        public Order SetDone(Order order)
        {
            var connection = providerFactory.CreateConnection();
            connection.ConnectionString = connectionString;

            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = "update orders set shippeddate = getdate() where orderid = @id ";

            var param = command.CreateParameter();
            param.ParameterName = "@id";
            param.Value = order.OrderId;

            command.Parameters.Add(param);

            command.ExecuteNonQuery();

            order.ShippedDate = DateTime.Now;
            order.Status = OrderStatus.Shipped;

            command.Dispose();
            connection.Close();

            return order;
        }

        public IEnumerable<CustomerOrderDetails> GetCustomerOrderDetails(int orderId)
        {
            var customerOrderDetails = new List<CustomerOrderDetails>();
            var connection = providerFactory.CreateConnection();
            connection.ConnectionString = connectionString;

            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "[dbo].[CustOrdersDetail]";

            var orderIdParam = command.CreateParameter();
            orderIdParam.ParameterName = "@OrderID";
            orderIdParam.Value = orderId;

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(orderIdParam);

            var dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                customerOrderDetails.Add(MapCustomerOrderDetails(dataReader));
            }
           
            dataReader.Close();
            command.Dispose();
            connection.Close();

            return customerOrderDetails;
        }

        public IEnumerable<CustomerOrderHistory> GetCustomerOrderHistory(string customerId)
        {
            var customerOrderHistory = new List<CustomerOrderHistory>();
            var connection = providerFactory.CreateConnection();
            connection.ConnectionString = connectionString;

            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "[dbo].[CustOrderHist]";

            var customerIdParam = command.CreateParameter();
            customerIdParam.ParameterName = "@CustomerID";
            customerIdParam.Value = customerId;

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(customerIdParam);

            var dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                customerOrderHistory.Add(MapCustomerOrderHistory(dataReader));
            }

            dataReader.Close();
            command.Dispose();
            connection.Close();

            return customerOrderHistory;
        }

        private Order MapOrder(DbDataReader dataReader)
        {
            var order = new Order();

            order.OrderId = dataReader.GetInt32(0);
            order.OrderDate = (!dataReader.IsDBNull(1)) ? (DateTime?)dataReader.GetDateTime(1) : null;
            order.RequiredDate = (!dataReader.IsDBNull(2)) ? (DateTime?)dataReader.GetDateTime(2) : null;
            order.ShippedDate = (!dataReader.IsDBNull(3)) ? (DateTime?)dataReader.GetDateTime(3) : null;

            if (order.OrderDate == null)
            {
                order.Status = OrderStatus.New;
            }
            else if (order.ShippedDate == null)
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

        private CustomerOrderDetails MapCustomerOrderDetails(DbDataReader dataReader)
        {
            var result = new CustomerOrderDetails();

            result.ProductName = dataReader.GetString(0);
            result.UnitPrice = dataReader.GetDecimal(1);
            result.Quantity = dataReader.GetInt16(2);
            result.Discount = dataReader.GetInt32(3);
            result.ExtendedPrice = dataReader.GetDecimal(4);

            return result;
        }

        private CustomerOrderHistory MapCustomerOrderHistory(DbDataReader dataReader)
        {
            var result = new CustomerOrderHistory();

            result.ProductName = dataReader.GetString(0);
            result.Total = dataReader.GetInt32(1);

            return result;
        }
    }
}