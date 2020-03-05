using System;
using System.Collections.Generic;
using System.IO;

namespace FileSystem.Visitor
{
    /// <summary>
    /// Class to find all file and directories from the File System.
    /// </summary>
    public class FileSystemVisitor : IFileSystemVisitor
    {
        public event Action Start;

        public event Action Finished;

        public event EventHandler<FileSystemEventArgs> FileFound;

        public event EventHandler<FileSystemEventArgs> DirectoryFound;

        public event EventHandler<FileSystemEventArgs> FilteredFileFound;

        public event EventHandler<FileSystemEventArgs> FilteredDirectoryFound;

        private readonly Func<string, bool> searchFilter;

        private readonly string startDirectoryLocation;

        private readonly List<string> foundItems = new List<string>();

        /// <summary>
        /// Constructor of <see cref="FileSystemVisitor"/>.
        /// </summary>
        /// <param name="startDirectoryLocation">The directory where to start the search.</param>
        /// <exception cref="ArgumentNullException">When the <paramref name="startDirectoryLocation"/> is not specified.</exception>
        /// <exception cref="ArgumentException">When <paramref name="startDirectoryLocation"/> contains invalid path to a directory.</exception>
        public FileSystemVisitor(string startDirectoryLocation) 
            : this(startDirectoryLocation, null)
        { }

        /// <summary>
        /// Constructor of <see cref="FileSystemVisitor"/>.
        /// </summary>
        /// <param name="startDirectoryLocation">The directory where to start the search.</param>
        /// <param name="filter">The <see cref="Func{T,TResult}"/> which help to filter the result list.</param>
        /// <exception cref="ArgumentNullException">When the <paramref name="startDirectoryLocation"/> is not specified.</exception>
        /// <exception cref="ArgumentException">When <paramref name="startDirectoryLocation"/> contains invalid path to a directory.</exception>
        public FileSystemVisitor(string startDirectoryLocation, Func<string, bool> filter)
        {
            if (startDirectoryLocation == null)
            {
                throw new ArgumentNullException(nameof(startDirectoryLocation));
            }

            if (!Directory.Exists(startDirectoryLocation))
            {
                throw new ArgumentException($"There is no such directory in  {startDirectoryLocation} path!",
                    nameof(startDirectoryLocation));
            }

            this.startDirectoryLocation = startDirectoryLocation;
            this.searchFilter = filter;
        }

        public void Search()
        {
            Start?.Invoke();

            Find();

            Finished?.Invoke();
        }

        public IEnumerable<string> GetNextFoundElement()
        {
            foreach (var item in foundItems)
            {
                yield return item;
            }
        }

        private void Find()
        {
            Stack<string> stack = new Stack<string>();
            
            stack.Push(startDirectoryLocation);
            foundItems.Clear();

            while (stack.Count > 0)
            {
                string directory = stack.Pop();

                var directories = Directory.GetDirectories(directory);
                var files = Directory.GetFiles(directory);

                foreach (var d in directories)
                {
                    OnFound(FoundTypeEnum.Directory, d);
                    stack.Push(d);

                    if (searchFilter != null)
                    {
                        if (searchFilter(d))
                        {
                            OnFound(FoundTypeEnum.FilteredDirectory, d);
                            foundItems.Add(d);
                        }
                    }
                    else
                    {
                        foundItems.Add(d);
                    }
                }

                foreach (var f in files)
                {
                    OnFound(FoundTypeEnum.File, f);

                    if (searchFilter != null)
                    {
                        if (searchFilter(f))
                        {
                            OnFound(FoundTypeEnum.FilteredFile, f);
                            foundItems.Add(f);
                        }
                    }
                    else
                    {
                    foundItems.Add(f);

                    }
                }
            }
        }

        private void OnFound(FoundTypeEnum foundType, string filePath)
        {
            EventHandler<FileSystemEventArgs> temp = null;

            switch (foundType)
            {
                case FoundTypeEnum.File:
                    temp = FileFound;
                    break;
                case FoundTypeEnum.Directory:
                    temp = DirectoryFound;
                    break;
                case FoundTypeEnum.FilteredFile:
                    temp = FilteredFileFound;
                    break;
                case FoundTypeEnum.FilteredDirectory:
                    temp = FilteredDirectoryFound;
                    break;
            }

            if (temp != null)
            {
                FileSystemEventArgs eventArgs = new FileSystemEventArgs(filePath);
                temp(this, eventArgs);
            }
        }
    }

    internal enum FoundTypeEnum
    {
        File,
        Directory,
        FilteredFile,
        FilteredDirectory
    }
}