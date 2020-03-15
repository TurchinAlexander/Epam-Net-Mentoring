using System.Collections.Generic;

namespace DataAccessLayer.Interfaces
{
    /// <summary>
    /// Interface to Repository.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Show all items from the repository.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Get an item from the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the item.</param>
        /// <returns>The <see cref="T"/>.</returns>
        T Get(int id);

        /// <summary>
        /// Add new item to the repository.
        /// </summary>
        /// <param name="item">Nex item.</param>
        /// <returns>The items which has been added.</returns>
        T Add(T item);

        /// <summary>
        /// Delete an item from the repository.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns>The item which has been deleted.</returns>
        T Delete(int id);
    }
}