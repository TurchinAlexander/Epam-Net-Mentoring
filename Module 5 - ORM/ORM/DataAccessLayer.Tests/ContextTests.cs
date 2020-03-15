using System.Linq;
using System.Reflection.PortableExecutable;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DataAccessLayer.Tests
{
    [TestFixture]
    public class ContextTests
    {
        private NorthwindContext context;

        [SetUp]
        public void SetUp()
        {
            NorthwindContextFactory factory = new NorthwindContextFactory();
            context = factory.CreateDbContext(new string[] {});
        }
        
        [Test]
        public void ChooseListOfOrdersWithSpeciedCategory_ReturnValidResult()
        {
            string categoryName = "";

            var query = 
                from o in context.Orders
                join cust in context.Customers on o.CustomerId equals cust.CustomerId
                join od in context.OrderDetails on o.OrderId equals od.OrderId
                join p in context.Products on od.ProductId equals p.ProductId
                join c in context.Categories on p.CategoryId equals c.CategoryId
                where c.CategoryName == categoryName
                select new {o, cust.ContactName, p.ProductName};

            Assert.True(true);
        }
    }
}