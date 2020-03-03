using System;

namespace CoreConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, please enter your name");
            Console.Write("My name is: ");

            var name = Console.ReadLine();

            Console.WriteLine(SharedProject.ShareClass.CreateGreeting(name));
            Console.WriteLine("Please enter any key...");
            Console.ReadLine();
        }
    }
}
