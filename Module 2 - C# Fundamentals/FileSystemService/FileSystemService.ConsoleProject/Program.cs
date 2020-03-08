using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FileSystemService.ConsoleProject.Configuration;
using FileSystemService.ConsoleProject.Globalization;
using FileSystemService.Monitor;
using FileSystemService.Monitor.MonitorEventArgs;
using Microsoft.VisualBasic.CompilerServices;

namespace FileSystemService.ConsoleProject
{
    class Program
    {
        private static IFileSystemMonitor fileMonitor;

        static void Main(string[] args)
        {
            ConsoleSection consoleSection = (ConsoleSection) ConfigurationManager.GetSection("consoleSection");
            string culture = consoleSection.CultureLocalization.Name;

            SetCulture(culture);
         
            Console.WriteLine(ConsoleResource.Greeting);

            InitializeFileMonitor();

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
            DoesCultureExists(culture);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
        }

        private static void InitializeFileMonitor()
        {
            fileMonitor = new FileSystemMonitor();

            fileMonitor.FileFound += FileFound;
            fileMonitor.FilterIsFound += FilterFound;
            fileMonitor.FilterIsNotFound += FilterNotFound;
            fileMonitor.FileIsTransferred += FileIsTransferred;
        }

        private static void FileFound(object sender, FileMonitorEventArgs e)
        {
            string message = string.Format(ConsoleResource.FileFound, e.FileName, e.Directory);
            Console.WriteLine(message);
        }

        private static void FilterFound(object sender, FilterMonitorEventArgs e)
        {
            string message = string.Format(ConsoleResource.FilterFound, e.FilterPattern, e.FileName);
            Console.WriteLine(message);
        }

        private static void FilterNotFound(object sender, FileMonitorEventArgs e)
        {
            string message = string.Format(ConsoleResource.FilterNotFound, e.FileName);
            Console.WriteLine(message);
        }

        private static void FileIsTransferred(object sender, TransferMonitorEventArgs e)
        {
            string message = string.Format(ConsoleResource.FileTrasnferred, e.FileName, e.Source, e.Destination);
            Console.WriteLine(message);
        }
    }
}