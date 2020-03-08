using System;
using FileSystemService.Monitor.Configuration;
using FileSystemService.Monitor.MonitorEventArgs;

namespace FileSystemService.Monitor
{
    public interface IFileSystemMonitor
    {
        /// <summary>
        /// Occurs when new file is place to the looked directories.
        /// </summary>
        event EventHandler<FileMonitorEventArgs> FileFound;

        /// <summary>
        /// Occurs when a file is matched for the filter.
        /// </summary>
        event EventHandler<FilterMonitorEventArgs> FilterIsFound;

        /// <summary>
        /// Occurs when any of the filter is not matched for the file.
        /// </summary>
        event EventHandler<FileMonitorEventArgs> FilterIsNotFound;

        /// <summary>
        /// Occurs when the file has been placed to <see cref="Filter.DestinationFolder"/>.
        /// </summary>
        event EventHandler<TransferMonitorEventArgs> FileIsTransferred;

        /// <summary>
        /// Start the execution of <see cref="FileSystemService"/> to look after the specified directories.
        /// </summary>
        void Start();

        /// <summary>
        /// End the execution of <see cref="IFileSystemMonitor"/> to look after the specified directories.
        /// </summary>
        void Stop();
    }
}