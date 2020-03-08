using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace FileSystemService.Monitor.MonitorEventArgs
{
    public class TransferMonitorEventArgs : BaseMonitorEventArgs
    {
        public string Source { get; }
        public string Destination { get; }

        public TransferMonitorEventArgs(string fileName, string source, string destination) 
            : base(fileName)
        {
            Contract.Requires<ArgumentException>(Directory.Exists(source));
            Contract.Requires<ArgumentException>(Directory.Exists(destination));

            Source = source;
            Destination = destination;
        }
    }
}