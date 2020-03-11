using System;
using System.Diagnostics.Contracts;

namespace FileSystemService.Monitor.MonitorEventArgs
{
    public abstract class BaseMonitorEventArgs : EventArgs
    {
        public string FileName { get; }

        protected BaseMonitorEventArgs(string fileName)
        {
            Contract.Requires<ArgumentNullException>(fileName != null);
            Contract.Requires<ArgumentException>(fileName != string.Empty);

            FileName = fileName;
        }
    }
}