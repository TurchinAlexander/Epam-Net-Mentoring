using System;
using System.Collections.Generic;

namespace FileSystem.Visitor
{
    /// <summary>
    /// Interface represent file system visitor
    /// </summary>
    public interface IFileSystemVisitor
    {
        /// <summary>
        /// Notify that the search is started.
        /// </summary>
        event Action Start;

        /// <summary>
        /// Notify that the search is finished.
        /// </summary>
        event Action Finished;

        /// <summary>
        /// Notify when a file is found in the searched directory.
        /// </summary>
        event EventHandler<FileSystemEventArgs> FileFound;
        
        /// <summary>
        /// Notify when a directory or sub directory is found in the searched directory.
        /// </summary>
        event EventHandler<FileSystemEventArgs> DirectoryFound;
        
        /// <summary>
        /// Notify when a file has not been filtered.
        /// </summary>
        event EventHandler<FileSystemEventArgs> FilteredFileFound;
       
        /// <summary>
        /// Notify when a directory has not been filtered.
        /// </summary>
        event EventHandler<FileSystemEventArgs> FilteredDirectoryFound;

        /// <summary>
        /// Start the search.
        /// </summary>
        void Search();

        /// <summary>
        /// Get the next found file or folder from the result list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetNextFoundElement();
    }
}