using Microsoft.Extensions.Logging;

namespace Nrrdio.Utilities.Loggers {
    public static class LoggerExtensions {
        public static ILoggingBuilder AddJsonFileLogger(this ILoggingBuilder builder, JsonFileLogger.Configuration config) =>
            builder.AddProvider(new LoggerProvider<JsonFileLogger, JsonFileLogger.Configuration> { Config = config });
    }
}
