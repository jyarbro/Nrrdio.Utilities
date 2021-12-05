using Microsoft.Extensions.Logging;

namespace Nrrdio.Utilities.Loggers.Contracts {
    public interface IAsyncLoggerConfig {
        public LogLevel LogLevel { init; }
    }
}
