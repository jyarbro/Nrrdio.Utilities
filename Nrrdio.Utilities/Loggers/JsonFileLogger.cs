using Microsoft.Extensions.Logging;
using Nrrdio.Utilities.Loggers.Contracts;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nrrdio.Utilities.Loggers {
    public class JsonFileLogger : INrrdioLogger, IDisposable {
        public string Name { private get; init; }

        public INrrdioLoggerConfig GenericConfig {
            get => Config;
            init => Config = value as Configuration;
        }
        Configuration Config { get; init; }

        public CancellationToken CancellationToken { private get; init; }

        ConcurrentQueue<LogEntry> WriteableQueue { get; } = new ConcurrentQueue<LogEntry>();

        string FilePath {
            get {
                if (string.IsNullOrEmpty(_FilePath)) {
                    _FilePath = $"{Config.FolderPath}\\{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log";
                }

                return _FilePath;
            }
        }
        string _FilePath;

        bool disposed;
        int counter;

        public JsonFileLogger() {
            MonitorQueue();
        }

        public IDisposable BeginScope<TState>(TState state) => default;

        public bool IsEnabled(LogLevel logLevel) => logLevel == Config.LogLevel;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter) {

            if (logLevel >= Config.LogLevel) {
                var message = $"{Config.Name} - {formatter(state, exception)}";

                WriteableQueue.Enqueue(new LogEntry {
                    EventId = eventId.Id,
                    LogLevel = logLevel,
                    Message = message,
                    Time = DateTime.Now,
                    Exception = exception
                });

                Debug.WriteLine(message);
            }
        }

        void PreparePath() {
            if (Config.RetainFileCount < 1) {
                throw new ArgumentException($"{nameof(Config.RetainFileCount)} must be greater than 0");
            }

            if (string.IsNullOrEmpty(Config.FolderPath)) {
                throw new ArgumentException($"{nameof(Config.FolderPath)} must contain a value");
            }

            var directoryInfo = Directory.CreateDirectory(Config.FolderPath);
            directoryInfo.Attributes = FileAttributes.Normal;

            var files = directoryInfo
                .GetFiles("*.log", SearchOption.TopDirectoryOnly)
                .OrderBy(o => o.CreationTime)
                .ToList();

            while (files.Count >= Config.RetainFileCount) {
                var fileInfo = files.First();
                File.SetAttributes(fileInfo.FullName, FileAttributes.Normal);
                File.Delete(fileInfo.FullName);
                files.Remove(fileInfo);
            }
        }

        /// <summary>
        /// Continuously monitors the queue for work.
        /// </summary>
        /// <param name="Text"></param>
        void MonitorQueue() {
            Task.Run(() => {
                PreparePath();
                
                while (!CancellationToken.IsCancellationRequested && !disposed) {
                    while (WriteableQueue.TryDequeue(out var logEntry)) {
                        // Check file size every 100 entries.
                        if (counter++ % 100 == 0) {
                            var fileInfo = new FileInfo(FilePath);

                            // If limit is reached, reset file path.
                            if (fileInfo.Exists && fileInfo.Length > (1024 * 1024 * Config.MaxFileSize)) {
                                _FilePath = "";
                                PreparePath();
                            }
                        }

                        JsonFiles.Write(FilePath, logEntry, true);
                    }

                    Thread.Sleep(100);
                }
            });
        }

        public void Dispose() => Dispose(true);
        ~JsonFileLogger() => Dispose(false);

        void Dispose(bool disposing) {
            if (!disposed) {
                var disposedTime = DateTime.Now;

                // Block thread until queue is empty or until time has passed.
                while (!WriteableQueue.IsEmpty && DateTime.Now < disposedTime.AddMilliseconds(500)) {
                    Thread.Sleep(50);
                }

                disposed = true;
            }
        }

        public record Configuration : INrrdioLoggerConfig {
            public string Name { get; init; }
            public LogLevel LogLevel { get; init; } = LogLevel.Information;
            public string FolderPath { get; init; }
            public int RetainFileCount { get; init; } = 5;
            public int MaxFileSize { get; init; } = 100;
        }

        public class LogEntry {
            public int EventId { get; set; }
            public LogLevel LogLevel { get; set; } = LogLevel.Information;
            public DateTime Time { get; set; }
            public string Message { get; set; }
            public Exception Exception { get; set; }
        }
    }
}
