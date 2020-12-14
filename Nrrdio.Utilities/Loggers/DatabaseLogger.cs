using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nrrdio.Utilities.Loggers.Contracts;
using System;
using System.Diagnostics;
using System.Threading;

namespace Nrrdio.Utilities.Loggers {
    public class DatabaseLogger : INrrdioLogger {
        public string Name { private get; init; }

        public INrrdioLoggerConfig GenericConfig {
            get => Config;
            init => Config = value as Configuration;
        }
        Configuration Config { get; init; }

        public CancellationToken CancellationToken { private get; init; }

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

                Config.Db.Add(new LogEntry {
                    EventId = eventId.Id,
                    LogLevel = logLevel,
                    Message = message,
                    Time = DateTime.Now,
                    Exception = exception
                });

                Config.Db.SaveChanges();

                Debug.WriteLine(message);
            }
        }

        public void Dispose() { }

        public record Configuration : INrrdioLoggerConfig {
            public string Name { get; init; }
            public LogLevel LogLevel { get; init; } = LogLevel.Information;
            public DbContext Db { get; init; }
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
