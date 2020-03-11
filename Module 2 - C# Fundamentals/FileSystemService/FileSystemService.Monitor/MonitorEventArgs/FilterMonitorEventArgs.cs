using System;
using System.Diagnostics.Contracts;

namespace FileSystemService.Monitor.MonitorEventArgs
{
    public class FilterMonitorEventArgs : BaseMonitorEventArgs
    {
        public string FilterPattern { get; }

        public FilterMonitorEventArgs(string fileName, string filterPattern) : base(fileName)
        {
            Contract.Requires<ArgumentNullException>(filterPattern != null);
            Contract.Requires<ArgumentException>(filterPattern == string.Empty);

            FilterPattern = filterPattern;
        }
    }
}