using Nrrdio.Utilities.Loggers.Contracts;

namespace Nrrdio.Utilities.Loggers;

/// <summary>
/// Returns log event to a registered handler. Useful when the handler is GUI based.
/// </summary>
public class HandlerLogger : IHandlerLogger {
	public event EventHandler<LogEntryEventArgs>? EntryAddedEvent;

	public string Name { get; init; } = "";
	public LogLevel LogLevel { get; init; } = LogLevel.Warning;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

    public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel;

	public void Log<TState>(
		LogLevel logLevel,
		EventId eventId,
		TState state,
		Exception? exception,
		Func<TState, Exception?, string> formatter
	) {
		if (IsEnabled(logLevel)) {
			EntryAddedEvent?.Invoke(this, new LogEntryEventArgs {
				LogEntry = new LogEntry {
					EventId = eventId.Id,
					LogLevel = logLevel,
					Name = Name,
					Message = formatter(state, exception),
					Time = DateTime.Now,
					SerializedException = exception is not null ? JsonSerializer.Serialize(exception) : string.Empty
				}
			});
		}
	}
}

public sealed class HandlerLoggerProvider : ILoggerProvider {
	public static HandlerLoggerProvider? Current { get; set; }

	ConcurrentDictionary<string, HandlerLogger> _Instances = new(StringComparer.OrdinalIgnoreCase);

	public LogLevel LogLevel { get; init; }

	public HandlerLoggerProvider() {
		if (Current is not null) {
			throw new InvalidOperationException($"There is already an instance of {nameof(HandlerLoggerProvider)}");
		}

		Current = this;
	}

	public ILogger CreateLogger(string categoryName) => 
		_Instances.GetOrAdd(categoryName, name => 
			new HandlerLogger {
				Name = name,
				LogLevel = LogLevel
			});

	public IHandlerLogger GetLogger(string instanceName) => _Instances[instanceName];

	public void RegisterEventHandler(EventHandler<LogEntryEventArgs> handler) {
		foreach (var instance in _Instances) {
			instance.Value.EntryAddedEvent += handler;
		}
	}

	public void DeregisterEventHandler(EventHandler<LogEntryEventArgs> handler) {
		foreach (var instance in _Instances) {
			instance.Value.EntryAddedEvent -= handler;
		}
	}

	public void Dispose() => _Instances.Clear();
}
