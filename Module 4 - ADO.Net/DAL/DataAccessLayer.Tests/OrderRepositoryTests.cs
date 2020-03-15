using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using DataAccessLayer.Interfaces;
using DataAccessLayer.OrderSection;
using DataAccessLayer.OrderSection.Models;
using NUnit.Framework;

namespace DataAccessLayer.Tests
{
    [TestFixture]
    public class OrderRepositoryTests
    {
        private static DbProviderFactory factory = SqlClientFactory.Instance;

        private static string connectionString =
            @"Server=DESKTOP-J66IAL7\SQLEXPRESS;Database=Northwind;Trusted_Connection = true";

        private static IOrderRepository orderRepository;


        [SetUp]
        public void SetUp()
        {
            orderRepository = new OrderRepository(factory, connectionString);
        }

        [Test]
        public void GetOrders_Nothing_EnumerableListOfOrders()
        {
            var result = orderRepository.GetAll()
                .ToList();

            Assert.True(result.Count > 0);
        }

        [Test]
        public void GetDetailedOrder_Nothing_OrderWithProducts()
        {
            var result = orderRepository.Get(10248);

            Assert.True(result != null);
            Assert.True(result.Products.Count > 0);
        }

        [Test]
        public void Add_Order_ReturnOrderWithOrderId()
        {
            var order = new Order();

            var updatedOrder = orderRepository.Add(order);

            Assert.True(updatedOrder.OrderId > 0);
            Assert.True(updatedOrder.RequiredDate.HasValue);
        }

        [Test]
        public void ChangeOrder_InvalidOperationException()
        {
            var order = orderRepository.Get(10248);

            Assert.Throws<InvalidOperationException>(() => order.OrderId = 10);
        }

        [Test]
        public void Delete_OrderIsDeleted()
        {
            var order = new Order();
            var updatedOrder = orderRepository.Add(order);

            var deletedOrder = orderRepository.Delete(updatedOrder.OrderId);

            Assert.True(updatedOrder.OrderId == deletedOrder.OrderId);
        }

        [Test]
        public void SetOrdered_OrderStatusIsChangedToOrdered()
        {
            var order = orderRepository.Add(new Order());

            var updatedOrder = orderRepository.SetOrderedDate(order.OrderId);

            Assert.True(updatedOrder.OrderDate != null
                        && updatedOrder.GetStatus() == OrderStatus.InProgress);
        }

        [Test]
        public void SetDone_OrderStatusIsChangedToShipped()
        {
            var order = orderRepository.Add(new Order());

            Order updatedOrder = orderRepository.SetOrderedDate(order.OrderId);
            updatedOrder = orderRepository.SetDone(updatedOrder.OrderId);

            Assert.True(updatedOrder.ShippedDate != null
                        && updatedOrder.GetStatus() == OrderStatus.Shipped);
        }

        [Test]
        public void GetCustomerOrderDetails_ReturnResult()
        {
            var result = orderRepository.GetCustomerOrderDetails(10248);

            Assert.True(result.Count() > 0);
        }

        [Test]
        public void GetCustomerOrderHistory_ReturnResult()
        {
            var result = orderRepository.GetCustomerOrderHistory("ALFKI");

            Assert.True(result.Count() > 0);
        }
    }
}