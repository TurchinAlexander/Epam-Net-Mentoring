using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FileSystemService.Monitor.Configuration;
using FileSystemService.Monitor.MonitorEventArgs;

namespace FileSystemService.Monitor
{
    public class FileSystemMonitor : IFileSystemMonitor
    {

        private readonly List<FileSystemWatcher> fileWatchers = new List<FileSystemWatcher>();

        private readonly MonitorSection monitorSection;

        /// <summary>
        /// Constructor of <see cref="FileSystemWatcher"/>.
        /// </summary>
        /// <remarks>The class uses <c>monitorSection</c> section in a configurationFile to set up his behaviour.</remarks>
        /// <exception cref="ConfigurationErrorsException"> occurs when the data in monitorSection in configuration file is incorrectly.</exception>
        public FileSystemMonitor()
        {
            monitorSection = (MonitorSection) ConfigurationManager.GetSection("monitorSection");
            InitializeWatchers();
        }

        public event EventHandler<FileMonitorEventArgs> FileFound;

        public event EventHandler<FilterMonitorEventArgs> FilterIsFound;

        public event EventHandler<FileMonitorEventArgs> FilterIsNotFound;

        public event EventHandler<TransferMonitorEventArgs> FileIsTransferred;

        public void Start()
        {
            foreach (var fileWatcher in fileWatchers)
            {
                fileWatcher.Created += NewFileInFileSystem;
            }
            
        }

        public void Stop()
        {
            foreach (var fileWatcher in fileWatchers)
            {
                fileWatcher.Created -= NewFileInFileSystem;
            }
        }

        private void InitializeWatchers()
        {
            foreach (WatchDirectory wd in monitorSection.WatchDirectories)
            {
                if (!Directory.Exists(wd.Path))
                {
                    throw new ConfigurationErrorsException(nameof(wd.Path));
                }

                FileSystemWatcher fileWatcher = new FileSystemWatcher()
                {
                    Path = wd.Path,
                    NotifyFilter = NotifyFilters.FileName,
                    EnableRaisingEvents = true,
                    IncludeSubdirectories = true
                };

                fileWatchers.Add(fileWatcher);
                
            }
        }

        private void RaiseEvent<T>(EventHandler<T> eventHandler, T eventArgs)
        {
            eventHandler?.Invoke(this, eventArgs);
        }

        private void NewFileInFileSystem(object sender, FileSystemEventArgs e)
        {
            Task.Run(() => StartExtractingFile(e.FullPath));
        }

        private void StartExtractingFile(string source)
        {

            string sourceFileName = Path.GetFileName(source);
            string sourceDirectory = Path.GetDirectoryName(source);
            bool filterNotFound = true;

            RaiseEvent(FileFound, new FileMonitorEventArgs(sourceFileName, sourceDirectory));

            foreach (Filter filter in monitorSection.Filters)
            {
                if (Regex.IsMatch(sourceFileName, filter.FilePattern, RegexOptions.IgnoreCase))
                {
                    filterNotFound = false;
                    
                    RaiseEvent(FilterIsFound, new FilterMonitorEventArgs(sourceFileName, filter.FilePattern));

                    string destinationFileName = PrepareFileName(filter, sourceFileName);
                    
                    File.Copy(source, Path.Combine(filter.DestinationFolder, destinationFileName), true);
                    RaiseEvent(FileIsTransferred, new TransferMonitorEventArgs(sourceFileName, sourceDirectory, filter.DestinationFolder));
                }
            }

            if (filterNotFound)
            {
                RaiseEvent(FilterIsNotFound, new FileMonitorEventArgs(sourceFileName, sourceDirectory));
            }
            
            File.Delete(source);
        }

        private string PrepareFileName(Filter filter, string source)
        {
            string sourceFileName = Path.GetFileNameWithoutExtension(source);
            string sourceExtension = Path.GetExtension(source);
            
            StringBuilder stringBuilder = new StringBuilder();

            if (filter.ShouldAddPositionNumber)
            {
                int count = Directory.GetFiles(filter.DestinationFolder).Length;

                stringBuilder.AppendFormat("{0}_", count++);
            }

            stringBuilder.Append(sourceFileName);

            if (filter.ShouldAddMovementDate)
            {
                stringBuilder.AppendFormat("_{0:ddMMyyyy}", DateTime.Now);
            }

            stringBuilder.Append(sourceExtension);

            return stringBuilder.ToString();
        }
    }
}