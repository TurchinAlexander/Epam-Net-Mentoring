using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;

namespace DataAccessLayer.Tests
{
    [TestFixture]
    public class OrderRepositoryTests
    {
        private static DbProviderFactory factory = SqlClientFactory.Instance;
        private static string connectionString = @"Server=DESKTOP-J66IAL7\SQLEXPRESS;Database=Northwind;Trusted_Connection = true";
        private static OrderRepository orderRepository;


        [SetUp]
        public void SetUp()
        {
            orderRepository = new OrderRepository(factory, connectionString);
        }

        [Test]
        public void GetOrders_Nothing_EnumerableListOfOrders()
        {
            var result = orderRepository.GetOrders();

            Assert.True(result.ToList().Count > 0);
        }
    }
}