using System;
using StackExchange.Redis;

namespace FibonacciCache
{
    public class RedisCache : IFibonacciCache
    {
        private ConnectionMultiplexer redis;
        private IDatabase db;

        public RedisCache()
        {
            redis = ConnectionMultiplexer.Connect("localhost");
            db = redis.GetDatabase();
        }

        public void AddItem(int key, int value)
        {
            db.StringSet(key.ToString(), value.ToString());
        }

        public int GetItem(int key)
        {
            return Convert.ToInt32(db.StringGet(key.ToString()));
        }

        public int RemoveItem(int key)
        {
            db.SetPop(key.ToString()).TryParse(out int result);

            return result;
        }
    }
}