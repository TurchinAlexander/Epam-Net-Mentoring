using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using DataAccessLayer.Interfaces;
using DataAccessLayer.OrderSection.Models;
using Microsoft.VisualBasic.FileIO;

namespace DataAccessLayer.OrderSection
{
    public class OrderRepository : IOrderRepository
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


        public IEnumerable<Order> GetAll()
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

            DbDataAdapter dataAdapter = providerFactory.CreateDataAdapter();

            dataAdapter.SelectCommand = command;
            DataTable table = new DataTable();

            dataAdapter.Fill(table);

            foreach (DataRow row in table.Rows)
            {
                orders.Add(Mapper.Map<Order>(row));
            }

            
            //dataReader.Close();
            EndExecution(command);

            return orders;
        }

        public Order Get(int id)
        {
            const int orderTable = 0;
            const int productTable = 1;

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

            var dataAdapter = providerFactory.CreateDataAdapter();
            DataSet dataSet = new DataSet();

            dataAdapter.SelectCommand = command;
            dataAdapter.Fill(dataSet);

            if (dataSet.Tables[orderTable].Rows.Count == 0)
            {
                return null;
            }

            var order = Mapper.Map<Order>(dataSet.Tables[orderTable].Rows[0]);

            if (dataSet.Tables[productTable].Rows.Count > 0)
            {
                order.Products = new List<Product>();

                foreach (DataRow row in dataSet.Tables[productTable].Rows)
                {
                    order.Products.Add(Mapper.Map<Product>(row));
                }
            }

            EndExecution(command);

            return order;
        }

        public Order Add(Order order)
        {
            var command = StartExecution();

            command.CommandText = "insert into orders (requireddate) values (getdate());" +
                                  "select @@IDENTITY;";

            var orderId = Convert.ToInt32(command.ExecuteScalar());

            EndExecution(command);

            return Get(orderId);
        }


        public Order Delete(int id)
        {
            var order = Get(id);

            if (order.GetStatus() == OrderStatus.Shipped)
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

        public Order SetOrderedDate(int orderId) =>
            UpdateStatus("UPDATE Orders SET OrderDate = GETDATE() WHERE OrderId = @id", orderId);

        public Order SetDone(int orderId) =>
            UpdateStatus("UPDATE Orders SET ShippedDate = GETDATE() WHERE OrderId = @id ", orderId);

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

            DbDataAdapter dataAdapter = providerFactory.CreateDataAdapter();
            DataTable table = new DataTable();

            dataAdapter.SelectCommand = command;
            dataAdapter.Fill(table);

            foreach (DataRow row in table.Rows)
            {
                customerOrderDetails.Add(Mapper.Map<CustomerOrderDetails>(row));
            }

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

            DbDataAdapter dataAdapter = providerFactory.CreateDataAdapter();
            DataTable table = new DataTable();

            dataAdapter.SelectCommand = command;
            dataAdapter.Fill(table);

            foreach (DataRow row in table.Rows)
            {
                customerOrderHistory.Add(Mapper.Map<CustomerOrderHistory>(row));
            }

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

        private Order UpdateStatus(string commandText, int orderId)
        {
            var command = StartExecution();

            command.CommandText = commandText;

            var param = command.CreateParameter();
            param.ParameterName = "@id";
            param.Value = orderId;

            command.Parameters.Add(param);
            command.ExecuteNonQuery();

            EndExecution(command);

            return Get(orderId);
        }
    }
}