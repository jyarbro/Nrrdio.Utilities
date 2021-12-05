﻿using Microsoft.Extensions.Logging;
using Nrrdio.Utilities.Loggers.Contracts;
using System;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Nrrdio.Utilities.Loggers {
    /// <summary>
    /// Returns log event to a registered handler. Useful when the handler is GUI based.
    /// </summary>
    public class HandlerLogger : IHandlerLogger {
        public event EventHandler<LogEntryEventArgs> EntryAddedEvent;

        public string Name { private get; init; }
        public LogLevel LogLevel { get; init; } = LogLevel.Information;

        public IDisposable BeginScope<TState>(TState state) => default;

        public bool IsEnabled(LogLevel logLevel) => logLevel == LogLevel;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
            var args = new LogEntryEventArgs {
                LogEntry = new LogEntry {
                    EventId = eventId.Id,
                    LogLevel = logLevel,
                    Name = Name,
                    Message = formatter(state, exception),
                    Time = DateTime.Now,
                    SerializedException = exception is not null ? JsonSerializer.Serialize(exception) : string.Empty
                }
            };

            EntryAddedEvent.Invoke(this, args);
        }
    }

    public sealed class HandlerLoggerProvider : ILoggerProvider {
        public static ConcurrentDictionary<string, HandlerLogger> Instances => new();

        public LogLevel LogLevel { get; init; }

        public ILogger CreateLogger(string categoryName) => Instances.GetOrAdd(categoryName, name => new HandlerLogger { 
            Name = name,
            LogLevel = LogLevel
        });

        public void Dispose() => Instances.Clear();
    }
}
