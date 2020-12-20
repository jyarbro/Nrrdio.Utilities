using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nrrdio.Utilities.Loggers;
using Nrrdio.Utilities.Loggers.Contracts;
using Nrrdio.Utilities.TestConsole.Models;
using Nrrdio.Utilities.TestConsole.Utilities;
using System.Threading;
using System.Threading.Tasks;

namespace Nrrdio.Utilities.TestConsole {
    class DatabaseLoggerTestHost : IHostedService {
        ILogger<DatabaseLoggerTestHost> Logger { get; init; }
        DatabaseErrorOnlyTests ErrorOnlyTests { get; init; }

        public DatabaseLoggerTestHost(
            ILogger<DatabaseLoggerTestHost> logger,
            IHostApplicationLifetime appLifetime,
            DatabaseErrorOnlyTests errorOnlyTests
        ) {
            Logger = logger;
            ErrorOnlyTests = errorOnlyTests;

            appLifetime.ApplicationStarted.Register(OnStarted);
            appLifetime.ApplicationStopping.Register(OnStopping);
            appLifetime.ApplicationStopped.Register(OnStopped);
        }

        public Task StartAsync(CancellationToken cancellationToken) {
            Logger.LogInformation($"{nameof(StartAsync)} has been called.");

            Logger.Log(LogLevel.Debug, "Debug");
            Logger.Log(LogLevel.Information, "Information");
            Logger.Log(LogLevel.Warning, "Warning");
            Logger.Log(LogLevel.Error, "Error");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            Logger.LogInformation($"{nameof(StopAsync)} has been called.");
            return Task.CompletedTask;
        }

        void OnStarted() {
            Logger.LogInformation($"{nameof(OnStarted)} has been called.");

            ErrorOnlyTests.RunTest();
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
                .ConfigureLogging(builder => {
                    builder.SetMinimumLevel(LogLevel.Warning);
                })
                .ConfigureServices((hostContext, services) => {
                    services.AddHostedService<DatabaseLoggerTestHost>();
                    services.AddSingleton<DataContext>();
                    services.AddSingleton<ILogEntryRepository, LogEntryTestRepository>();
                    services.AddSingleton<ILoggerProvider, DatabaseLoggerProvider>();
                    services.AddScoped<DatabaseErrorOnlyTests>();
                    services.AddLogging();
                });
    }
}
