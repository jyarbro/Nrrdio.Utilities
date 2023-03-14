namespace Nrrdio.Utilities.Loggers.Contracts;

public interface IHandlerLogger : ILogger {
	event EventHandler<LogEntryEventArgs> EntryAddedEvent;

	string Name { init; }
	LogLevel LogLevel { get; init; }
}
