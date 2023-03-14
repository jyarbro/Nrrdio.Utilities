namespace Nrrdio.Utilities.Loggers.Contracts;

public interface IAsyncLogger : ILogger, IDisposable {
	public string Name { init; }
	public IAsyncLoggerConfig GenericConfig { init; }
	public CancellationToken CancellationToken { init; }
}
