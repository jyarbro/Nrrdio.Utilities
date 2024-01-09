using Nrrdio.Utilities.Loggers.Contracts;

namespace Nrrdio.Utilities.Loggers;

public class DatabaseLogger : ILogger {
	public string Name { private get; init; } = "";

	public LogLevel LogLevel { get; init; } = LogLevel.Information;

	ILogEntryRepository Repository { get; init; }

	public DatabaseLogger(
		ILogEntryRepository repository
	) {
		Repository = repository;
	}

	public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

	public bool IsEnabled(LogLevel logLevel) => logLevel == LogLevel;

	public void Log<TState>(
		LogLevel logLevel,
		EventId eventId,
		TState state,
		Exception? exception,
		Func<TState, Exception?, string> formatter) {

		if (logLevel >= LogLevel) {
			Repository.Add(new LogEntry {
				EventId = eventId.Id,
				LogLevel = logLevel,
				Name = Name,
				Message = formatter(state, exception),
				Time = DateTime.Now,
				SerializedException = exception is not null ? JsonSerializer.Serialize(exception) : string.Empty
			});
		}
	}

	public void Dispose() { }
}

public sealed class DatabaseLoggerProvider : ILoggerProvider {
	readonly ILogEntryRepository Repository;
	static ConcurrentDictionary<string, DatabaseLogger> Instances => new();

	public LogLevel LogLevel { get; set; }

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
