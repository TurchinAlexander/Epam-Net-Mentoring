using System;
using System.Diagnostics.Contracts;

namespace FileSystemService.Monitor.MonitorEventArgs
{
    public class FileMonitorEventArgs : BaseMonitorEventArgs
    {
        public string Directory { get; set; }
        
        public FileMonitorEventArgs(string fileName, string directory) : base(fileName)
        {
            Contract.Requires<ArgumentNullException>(directory != null);
            Contract.Requires<ArgumentException>(directory != string.Empty);

            Directory = directory;
        }
    }
}