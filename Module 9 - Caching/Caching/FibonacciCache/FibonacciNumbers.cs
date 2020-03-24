using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace FibonacciCache
{
    public class FibonacciNumbers
    {
        private IFibonacciCache cache;
        private int index = 0;

        public FibonacciNumbers(IFibonacciCache cache)
        {
            this.cache = cache;
            
            this.cache.AddItem(++index, 1);
            this.cache.AddItem(++index, 1);
        }

        public int Calculate(int numberIndex)
        {
            if (numberIndex < 0)
            {
                throw new ArgumentException("Number index could not be less or equal to zero!");
            }

            if (numberIndex <= index)
            {
                return cache.GetItem(numberIndex);
            }

            int number = cache.GetItem(index);
            int numberPrev = cache.GetItem(index - 1);

            while (numberIndex > index)
            {
                int result = number + numberPrev;

                cache.AddItem(++index, result);

                numberPrev = number;
                number = result;
            }

            return cache.GetItem(numberIndex);
        }


    }
}