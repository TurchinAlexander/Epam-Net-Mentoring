using System;
using StringToInt.Library;

namespace StringToInt.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the number in decimal system less than !");
            
            string text = Console.ReadLine();

            if (StringToIntConverter.TryConvert(text, out int result))
            {
                Console.WriteLine("Correct input");
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine("Incorrect input!");
            }

            Console.ReadLine();
        }
    }
}