using System.Collections.Generic;
using DataAccessLayer.OrderSection.Models;

namespace DataAccessLayer.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        /// <summary>
        /// Set OrderedDate of the order.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order.</param>
        /// <returns></returns>
        Order SetOrderedDate(int orderId);

        /// <summary>
        /// Set ShippedDate of the order.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Order SetDone(int orderId);

        /// <summary>
        /// Call CustOrderDetails stored procedure.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order.</param>
        /// <returns>The <see cref="CustomerOrderDetails"/>.</returns>
        IEnumerable<CustomerOrderDetails> GetCustomerOrderDetails(int orderId);

        /// <summary>
        /// Call CustOrderHist stored procedure.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer.</param>
        /// <returns>The <see cref="CustomerOrderHistory"/>.</returns>
        IEnumerable<CustomerOrderHistory> GetCustomerOrderHistory(string customerId);
    }
}