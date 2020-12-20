using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nrrdio.Utilities.Loggers.Contracts;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;

namespace Nrrdio.Utilities.Loggers {
    public class DatabaseLogger : ILogger {
        public string Name { private get; init; }

        public LogLevel LogLevel { get; init; } = LogLevel.Information;

        ILogEntryRepository Repository { get; init; }

        public DatabaseLogger(
            ILogEntryRepository repository
        ) {
            Repository = repository;
        }

        public IDisposable BeginScope<TState>(TState state) => default;

        public bool IsEnabled(LogLevel logLevel) => logLevel == LogLevel;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter) {

            if (logLevel >= LogLevel) {
                var message = $"{Name} - {formatter(state, exception)}";

                Repository.Add(new LogEntry {
                    EventId = eventId.Id,
                    LogLevel = logLevel,
                    Message = message,
                    Time = DateTime.Now,
                    SerializedException = exception is not null ? JsonSerializer.Serialize(exception) : string.Empty
                });

                Debug.WriteLine(message);
            }
        }

        public void Dispose() { }
    }

    public sealed class DatabaseLoggerProvider : ILoggerProvider {
        readonly ILogEntryRepository Repository;

        public LogLevel LogLevel { get; set; }

        ConcurrentDictionary<string, DatabaseLogger> Instances => new ConcurrentDictionary<string, DatabaseLogger>();

        public DatabaseLoggerProvider(
            ILogEntryRepository repository
        ) {
            Repository = repository;
        }

        public ILogger CreateLogger(string categoryName) => Instances.GetOrAdd(categoryName, name => new DatabaseLogger(Repository) {
            Name = name,
            LogLevel = LogLevel
        });

        public void Dispose() {
            Instances.Clear();
        }
    }
}
