using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FibonacciCache;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int result;
            var fibonacci = new FibonacciNumbers(new RedisCache());

            result = fibonacci.Calculate(2);
            Console.WriteLine(result);

            result = fibonacci.Calculate(7);
            Console.WriteLine(result);

            result = fibonacci.Calculate(5);
            Console.WriteLine(result);
        }
    }
}
