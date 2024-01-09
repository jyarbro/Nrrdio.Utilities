namespace Nrrdio.Utilities.Loggers;

public class LogEntry {
	public int Id { get; set; }
	public int EventId { get; set; }
	public LogLevel LogLevel { get; set; } = LogLevel.Information;
	public DateTime Time { get; set; }
	public string Name { get; set; } = "";
	public string Message { get; set; } = "";
	public string SerializedException { get; set; } = "";
}
