using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Nrrdio.Utilities.Loggers {
    // source: https://docs.microsoft.com/en-us/dotnet/core/extensions/custom-logging-provider
    public class ColorConsoleLogger : ILogger {
        public string Name { private get; init; }

        public int EventId { get; set; }
        public LogLevel LogLevel { get; set; } = LogLevel.Information;

        public IDisposable BeginScope<TState>(TState state) => default;
        public bool IsEnabled(LogLevel logLevel) => logLevel == LogLevel;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter) {

            if (!IsEnabled(logLevel)) {
                return;
            }

            if (EventId == 0 || EventId == eventId.Id) {
                ConsoleColor originalColor = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[{eventId.Id,2}: {logLevel,-12}]");

                Console.ForegroundColor = originalColor;
                Console.WriteLine($"     {Name} - {formatter(state, exception)}");
            }
        }
    }

    public sealed class ColorConsoleLoggerProvider : ILoggerProvider {
        static ConcurrentDictionary<string, ColorConsoleLogger> Instances => new();

        public ILogger CreateLogger(string categoryName) => Instances.GetOrAdd(categoryName, name => new ColorConsoleLogger { Name = name });
        public void Dispose() => Instances.Clear();
    }
}
