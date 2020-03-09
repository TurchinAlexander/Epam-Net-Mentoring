using System;

namespace FirstCharacter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, please enter a text and I will show the first character in it.");
            int badCount = 0;
            const int MaxBadCount = 2;

            do
            {
                string text = Console.ReadLine();
                if (string.IsNullOrEmpty(text))
                {
                    Console.WriteLine("You didn't write anything. Please try again.");
                    badCount++;
                }
                else
                {
                    Console.WriteLine($"Aha, the first letter in the line is {text[0]}. Go another one!");
                }
            } while (badCount < MaxBadCount);
            
            Console.WriteLine("I don't want to play with you anymore!");
        }
    }
}