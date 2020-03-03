using System;
using FileSystem.Visitor;

namespace FileSystem.Cosole
{
    class Program
    {
        static void Main(string[] args)
        {
            string startLocation = @"E:\EPAM";

            IFileSystemVisitor fileSystemVisitor = new FileSystemVisitor(startLocation);
            fileSystemVisitor.Search();

            foreach (var file in fileSystemVisitor.GetNextFoundElement())
            {
                Console.WriteLine(file);
            }

            Console.ReadLine();

        }
    }
}
