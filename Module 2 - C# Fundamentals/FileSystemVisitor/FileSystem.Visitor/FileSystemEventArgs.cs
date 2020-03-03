using System;

namespace FileSystem.Visitor
{
    public class FileSystemEventArgs : EventArgs
    {
        public string FilePath { get; }

        public FileSystemEventArgs(string filePath)
        {
            this.FilePath = filePath;
        }
    }
}