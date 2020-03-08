using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FileSystemService.ConsoleProject.Configuration;
using FileSystemService.ConsoleProject.Globalization;
using FileSystemService.Monitor;

namespace FileSystemService.ConsoleProject
{
    class Program
    {
        private static IFileSystemMonitor fileMonitor;

        static void Main(string[] args)
        {
            ConsoleSection consoleSection = (ConsoleSection) ConfigurationManager.GetSection("consoleSection");
            string culture = consoleSection.CultureLocalization.Name;

            DoesCultureExists(culture);
            SetCulture(culture);
         
            Console.WriteLine(ConsoleResource.Greeting);

            fileMonitor = new FileSystemMonitor();

            Console.WriteLine(ConsoleResource.StartWork);
            
            //Task.Run(() => fileMonitor.Start());
            fileMonitor.Start();

            Console.WriteLine(ConsoleResource.Hint);

            var shouldNotExit = true;
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey();

                if (((key.Modifiers & ConsoleModifiers.Control) != 0)
                    && (key.Key.Equals(ConsoleKey.C)
                        || key.Key.Equals(ConsoleKey.Pause)))
                {
                    shouldNotExit = false;
                }
            } while (shouldNotExit);

            fileMonitor.Stop();

            Console.WriteLine(ConsoleResource.EndWork);
            Console.WriteLine(ConsoleResource.EnterAnyKey);
            Console.ReadKey();
        }
        
        private static void DoesCultureExists(string cultureName)
        {
            // string cultureName = (string) value;
            
            var found = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Any(culture => string.Equals(culture.Name, cultureName, StringComparison.InvariantCultureIgnoreCase));

            if (!found)
            {
                throw new ArgumentException(nameof(cultureName));
            }
        }

        private static void SetCulture(string culture)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
        }
    }
}