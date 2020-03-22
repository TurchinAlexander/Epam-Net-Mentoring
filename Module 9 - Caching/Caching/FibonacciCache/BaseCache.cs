using System;
using System.Runtime.Caching;

namespace FibonacciCache
{
    public class BaseCache : IFibonacciCache
    {
        MemoryCache cache = new MemoryCache("Base cache.");

        public void AddItem(int key, int value)
        {
            cache.Add(key.ToString(), value.ToString(), null);

        }

        public int GetItem(int key)
        {
            object value = cache.Get(key.ToString(), null);

            return Convert.ToInt32(value);
        }

        public int RemoveItem(int key)
        {
            throw new System.NotImplementedException();
        }
    }
}