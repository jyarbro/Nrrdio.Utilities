using Microsoft.Extensions.Logging;

namespace Nrrdio.Utilities.TestConsole.Utilities {
    public class DatabaseErrorOnlyTests {
        ILogger<DatabaseErrorOnlyTests> Logger { get; init; }

        public DatabaseErrorOnlyTests(
            ILogger<DatabaseErrorOnlyTests> logger
        ) {
            Logger = logger;
        }

        public void RunTest() {
            Logger.LogInformation($"{nameof(RunTest)} has been called.");

            Logger.LogDebug("Oh Kay.");
            Logger.LogInformation("Oh yeah.");
            Logger.LogWarning("Oh uh.");
            Logger.LogError("Oh no.");
        }
    }
}
