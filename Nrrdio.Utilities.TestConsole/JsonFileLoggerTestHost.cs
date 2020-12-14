using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nrrdio.Utilities.Loggers;
using System.Threading;
using System.Threading.Tasks;

namespace Nrrdio.Utilities.TestConsole {
    class JsonFileLoggerTestHost : IHostedService {
        public ILogger<JsonFileLoggerTestHost> Logger { private get; init; }

        public JsonFileLoggerTestHost(
            ILogger<JsonFileLoggerTestHost> logger,
            IHostApplicationLifetime appLifetime
        ) {
            Logger = logger;

            appLifetime.ApplicationStarted.Register(OnStarted);
            appLifetime.ApplicationStopping.Register(OnStopping);
            appLifetime.ApplicationStopped.Register(OnStopped);
        }

        public Task StartAsync(CancellationToken cancellationToken) {
            Logger.LogInformation($"{nameof(StartAsync)} has been called.");

            Logger.Log(LogLevel.Information, "Information");
            Logger.Log(LogLevel.Warning, "Warning");
            Logger.Log(LogLevel.Debug, "Debug");
            Logger.Log(LogLevel.Error, "Error");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            Logger.LogInformation($"{nameof(StopAsync)} has been called.");
            return Task.CompletedTask;
        }

        void OnStarted() {
            Logger.LogInformation($"{nameof(OnStarted)} has been called.");
        }

        void OnStopping() {
            Logger.LogInformation($"{nameof(OnStopping)} has been called.");
        }

        void OnStopped() {
            Logger.LogInformation($"{nameof(OnStopped)} has been called.");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host
                .CreateDefaultBuilder(args)
                .ConfigureLogging(builder =>
                    builder.ClearProviders()
                        // Test using AddProvider
                        .AddProvider(
                            new LoggerProvider<JsonFileLogger, JsonFileLogger.Configuration> {
                                Config = new JsonFileLogger.Configuration {
                                    LogLevel = LogLevel.Error,
                                    FolderPath = "First",
                                    RetainFileCount = 1
                                }
                            })
                        // Test using extension method
                        .AddJsonFileLogger(new JsonFileLogger.Configuration {
                            LogLevel = LogLevel.Warning,
                            FolderPath = "Second",
                            RetainFileCount = 2
                        })
                )
                .ConfigureServices((hostContext, services) => {
                    services.AddHostedService<JsonFileLoggerTestHost>();
                });
    }
}
