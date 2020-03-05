using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace FileSystem.Visitor.Tests
{
    [TestFixture]
    public class FileSystemVisitorTest
    {
        private readonly string validFolder = @"E:\EPAM\Module 2 - C# Fundamentals\FileSystemVisitor\FileSystem.Visitor.TestFolder";

        private readonly string[] filesWithoutFilter = 
        {
           @"E:\EPAM\Module 2 - C# Fundamentals\FileSystemVisitor\FileSystem.Visitor.TestFolder\TestFolder",
           @"E:\EPAM\Module 2 - C# Fundamentals\FileSystemVisitor\FileSystem.Visitor.TestFolder\123.txt",
           @"E:\EPAM\Module 2 - C# Fundamentals\FileSystemVisitor\FileSystem.Visitor.TestFolder\234.csv",
           @"E:\EPAM\Module 2 - C# Fundamentals\FileSystemVisitor\FileSystem.Visitor.TestFolder\JustAnotherFile.txt",
           @"E:\EPAM\Module 2 - C# Fundamentals\FileSystemVisitor\FileSystem.Visitor.TestFolder\Name.txt",
           @"E:\EPAM\Module 2 - C# Fundamentals\FileSystemVisitor\FileSystem.Visitor.TestFolder\TestFolder\fileUnderSubFolder.txt"
        };

        private readonly string[] directories =
        {
            @"E:\EPAM\Module 2 - C# Fundamentals\FileSystemVisitor\FileSystem.Visitor.TestFolder\TestFolder"
        };

        private readonly string[] csvFiles =
        {
            @"E:\EPAM\Module 2 - C# Fundamentals\FileSystemVisitor\FileSystem.Visitor.TestFolder\234.csv"
        };

        private readonly string[] onlyDigitsItems =
        {
            @"E:\EPAM\Module 2 - C# Fundamentals\FileSystemVisitor\FileSystem.Visitor.TestFolder\123.txt",
            @"E:\EPAM\Module 2 - C# Fundamentals\FileSystemVisitor\FileSystem.Visitor.TestFolder\234.csv"
        };

        [Test]
        public void Constructor_NullStartDirectoryLocationGiven_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new FileSystemVisitor(null));
        }

        [TestCase("C:\\NotExistedFolder")]
        public void Constructor_InvalidStartDirectoryLocationGiven_ShouldThrowArgumentException(string directory)
        {
            Assert.Throws<ArgumentException>(() => new FileSystemVisitor(directory));
        }

        [Test]
        public void Constructor_ValidStartDirectoryLocationGiven_ShouldRunWithoutAnyException()
        {
            Assert.DoesNotThrow(() => new FileSystemVisitor(validFolder));
        }

        [Test]
        public void GetNextFoundElement_ValidStartDirectoryLocationWithoutAnyFilter_AllFilesInTheDirectoryInOutput()
        {
            var fileVisitor = new FileSystemVisitor(validFolder);
            
            fileVisitor.Search();

            var foundItems = fileVisitor.GetNextFoundElement().ToArray();

            Assert.True(CompareTwoArrays(filesWithoutFilter, foundItems));
        }

        [Test]
        public void GetNextFoundElement_ValidStartDirectoryLocationWithOnlyDirectoryFilter_OnlyDirectoriesInOutput()
        {
            var fileVisitor = new FileSystemVisitor(validFolder, fileName => Directory.Exists(fileName));

            fileVisitor.Search();

            var foundItems = fileVisitor.GetNextFoundElement().ToArray();

            Assert.True(CompareTwoArrays(directories, foundItems));
        }

        [Test]
        public void GetNextFoundElement_ValidStartDirectoryLocationWithOnlyCsvFilesFilter_OnlyFilesWithCsvExtensionInOutput()
        {
            var fileVisitor = new FileSystemVisitor(validFolder, filePath =>
            {
                var extension = Path.GetExtension(filePath);

                return (extension.Equals(".csv", StringComparison.InvariantCultureIgnoreCase));
            });

            fileVisitor.Search();

            var foundItems = fileVisitor.GetNextFoundElement().ToArray();

            Assert.True(CompareTwoArrays(csvFiles, foundItems));
        }

        [Test]
        public void GetNextFoundElement_ValidStartDirectoryLocationWithOnlyDigitsFileName_OnlyFilesWithDigitsInOutput()
        {
            var fileVisitor = new FileSystemVisitor(validFolder, filePath =>
            {
                var fileName = Path.GetFileName(filePath);

                return Regex.IsMatch(fileName, "[0-9]+");
            });

            fileVisitor.Search();

            var foundItems = fileVisitor.GetNextFoundElement().ToArray();

            Assert.True(CompareTwoArrays(onlyDigitsItems, foundItems));
        }

        private bool CompareTwoArrays(string[] expectedResult, string[] actualResult)
        {
            if (expectedResult.Length != actualResult.Length)
            {
                return false;
            }

            for (int i = 0; i < expectedResult.Length; i++)
            {
                if (!expectedResult[i].Equals(actualResult[i]))
                {
                    return false;
                }
            }

            return true;
        }
}
}