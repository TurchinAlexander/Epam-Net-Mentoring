using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using DataAccessLayer.OrderSection.Models;

namespace DataAccessLayer.OrderSection
{
    public class OrderRepository
    {
        private readonly DbProviderFactory providerFactory;
        private readonly string connectionString;
        private readonly DbConnection connection;

        public OrderRepository(DbProviderFactory providerFactory, string connectionString)
        {
            this.providerFactory = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(providerFactory));

            connection = providerFactory.CreateConnection();
            connection.ConnectionString = connectionString;
        }

        public Order Add(Order order)
        {
            var command = StartExecution();

            command.CommandText = "insert into orders (requireddate) values (getdate());" +
                                  "select @@IDENTITY;";

            order.OrderId = Convert.ToInt32(command.ExecuteScalar());
            order.RequiredDate = DateTime.Now;

            EndExecution(command);

            return order;
        }

        public IEnumerable<Order> GetOrders()
        {
            var orders = new List<Order>();

            var command = StartExecution();

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
                orders.Add(Mapper.ToOrder(dataReader));
            }
            
            dataReader.Close();
            EndExecution(command);

            return orders;
        }

        public Order GetDetailedOrder(int id)
        {
            var command = StartExecution();

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

            var paramId = command.CreateParameter();
            paramId.ParameterName = "@id";
            paramId.Value = id;

            command.Parameters.Add(paramId);

            var dataReader = command.ExecuteReader();
            if (!dataReader.HasRows)
            {
                return null;
            }

            dataReader.Read();
            var order = Mapper.ToOrder(dataReader);

            dataReader.NextResult();
            order.Products = new List<Product>();

            while (dataReader.Read())
            {
                order.Products.Add(Mapper.ToProduct(dataReader));
            }

            dataReader.Close();
            EndExecution(command);

            return order;
        }

        public Order Delete(int id)
        {
            var order = GetDetailedOrder(id);

            if (order.Status == OrderStatus.Shipped)
            {
                return order;
            }

            var command = StartExecution();

            command.CommandText = "delete orders where orderID = @id";

            var param = command.CreateParameter();
            param.ParameterName = "@id";
            param.Value = id;

            command.Parameters.Add(param);
            command.ExecuteNonQuery();

            EndExecution(command);

            return order;
        }

        public Order SetOrderedDate(Order order)
        {
            var command = StartExecution();

            command.CommandText = "update orders set orderdate = getdate() where orderid = @id ";

            var param = command.CreateParameter();
            param.ParameterName = "@id";
            param.Value = order.OrderId;

            command.Parameters.Add(param);
            command.ExecuteNonQuery();

            EndExecution(command);

            return GetDetailedOrder(order.OrderId);
        }

        public Order SetDone(Order order)
        {
            var command = StartExecution();

            command.CommandText = "update orders set shippeddate = getdate() where orderid = @id ";

            var param = command.CreateParameter();
            param.ParameterName = "@id";
            param.Value = order.OrderId;

            command.Parameters.Add(param);
            command.ExecuteNonQuery();

            EndExecution(command);

            return GetDetailedOrder(order.OrderId);
        }

        public IEnumerable<CustomerOrderDetails> GetCustomerOrderDetails(int orderId)
        {
            var customerOrderDetails = new List<CustomerOrderDetails>();
            var command = StartExecution();

            command.CommandText = "[dbo].[CustOrdersDetail]";
            command.CommandType = CommandType.StoredProcedure;

            var orderIdParam = command.CreateParameter();
            orderIdParam.ParameterName = "@OrderID";
            orderIdParam.Value = orderId;

            
            command.Parameters.Add(orderIdParam);

            var dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                customerOrderDetails.Add(Mapper.ToCustomerOrderDetails(dataReader));
            }
           
            dataReader.Close();
            EndExecution(command);

            return customerOrderDetails;
        }

        public IEnumerable<CustomerOrderHistory> GetCustomerOrderHistory(string customerId)
        {
            var customerOrderHistory = new List<CustomerOrderHistory>();
            var command = StartExecution();

            command.CommandText = "[dbo].[CustOrderHist]";
            command.CommandType = CommandType.StoredProcedure;

            var customerIdParam = command.CreateParameter();
            customerIdParam.ParameterName = "@CustomerID";
            customerIdParam.Value = customerId;

            command.Parameters.Add(customerIdParam);

            var dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                customerOrderHistory.Add(Mapper.ToCustomerOrderHistory(dataReader));
            }

            dataReader.Close();
            EndExecution(command);

            return customerOrderHistory;
        }

        private DbCommand StartExecution()
        {
            connection.Open();

            return connection.CreateCommand();
        }

        private void EndExecution(DbCommand command)
        {
            command.Dispose();
            connection.Close();
        }

        private static class Mapper
        {
            public static Order ToOrder(DbDataReader dataReader)
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

            public static Product ToProduct(DbDataReader dataReader)
            {
                var product = new Product();

                product.ProductId = dataReader.GetInt32(0);
                product.ProductName = dataReader.GetString(1);

                return product;
            }

            public static CustomerOrderDetails ToCustomerOrderDetails(DbDataReader dataReader)
            {
                var result = new CustomerOrderDetails();

                result.ProductName = dataReader.GetString(0);
                result.UnitPrice = dataReader.GetDecimal(1);
                result.Quantity = dataReader.GetInt16(2);
                result.Discount = dataReader.GetInt32(3);
                result.ExtendedPrice = dataReader.GetDecimal(4);

                return result;
            }

            public static CustomerOrderHistory ToCustomerOrderHistory(DbDataReader dataReader)
            {
                var result = new CustomerOrderHistory();

                result.ProductName = dataReader.GetString(0);
                result.Total = dataReader.GetInt32(1);

                return result;
            }
        }
    }
}