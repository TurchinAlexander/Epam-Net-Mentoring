namespace FibonacciCache
{
    public interface IFibonacciCache
    {
        /// <summary>
        /// Add item to the cache.
        /// </summary>
        /// <param name="key">Unique identifier of the inserted item.</param>
        /// <param name="value">The value of the inserted item.</param>
        void AddItem(int key, int value);

        /// <summary>
        /// Get the value of the item in the cache by its unique identifier.
        /// </summary>
        /// <param name="key">Unique identifier.</param>
        /// <returns></returns>
        int GetItem(int key);

        /// <summary>
        /// Remove the item from the cache.
        /// </summary>
        /// <param name="key">Unique identifier of the item.</param>
        /// <returns></returns>
        int RemoveItem(int key);
    }
}