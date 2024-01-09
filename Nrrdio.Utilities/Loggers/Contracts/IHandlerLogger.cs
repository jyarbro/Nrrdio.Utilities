namespace Nrrdio.Utilities.Loggers.Contracts;

public interface IHandlerLogger : ILogger {
	event EventHandler<LogEntryEventArgs> EntryAddedEvent;

	string Name { get; init; }
	LogLevel LogLevel { get; init; }
}
