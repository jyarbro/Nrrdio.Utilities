using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Nrrdio.Utilities.Loggers {
    public class DatabaseLogger : ILogger {
        Configuration Config { get; init; }
        DbContext DbContext { get; init; }

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

                DbContext.Add(new LogEntry {
                    EventId = eventId.Id,
                    LogLevel = logLevel,
                    Message = message,
                    Time = DateTime.Now,
                    Exception = exception
                });

                DbContext.SaveChanges();

                Debug.WriteLine(message);
            }
        }

        public record Configuration {
            public string Name { get; init; }
            public LogLevel LogLevel { get; init; } = LogLevel.Information;
        }

        public class LogEntry {
            public int EventId { get; set; }
            public LogLevel LogLevel { get; set; } = LogLevel.Information;
            public DateTime Time { get; set; }
            public string Message { get; set; }
            public Exception Exception { get; set; }
        }

        public sealed class Provider : ILoggerProvider {
            public Configuration Config { private get; init; }

            ConcurrentDictionary<string, DatabaseLogger> Instances => new ConcurrentDictionary<string, DatabaseLogger>();

            public ILogger CreateLogger(string categoryName) => Instances.GetOrAdd(categoryName, name => new DatabaseLogger { Config = Config with { Name = name } });
            public void Dispose() {
                Instances.Clear();
            }
        }
    }

    public static class DatabaseLoggerExtensions {
        public static ILoggingBuilder AddDatabaseLogger(this ILoggingBuilder builder, DatabaseLogger.Configuration config) =>
            builder.AddProvider(new DatabaseLogger.Provider { Config = config });
    }
}
