using Microsoft.Extensions.Logging;

namespace Nrrdio.Utilities.Loggers.Contracts {
    public interface INrrdioLoggerConfig {
        public LogLevel LogLevel { init; }
    }
}
