using System;
using FileSystemService.Monitor.MonitorEventArgs;

namespace FileSystemService.Monitor
{
    public interface IFileSystemMonitor
    {
        
        event EventHandler<FileMonitorEventArgs> FileFound;

        event EventHandler<FilterMonitorEventArgs> FilterIsFound;

        event EventHandler<FileMonitorEventArgs> FilterIsNotFound;

        event EventHandler<TransferMonitorEventArgs> FileIsTransferred;

        /// <summary>
        /// Start the execution of <see cref="FileSystemService"/> to look after the specified folders.
        /// </summary>
        void Start();

        /// <summary>
        /// End the execution of <see cref="IFileSystemMonitor"/> to look after the specified folders.
        /// </summary>
        void Stop();
    }
}