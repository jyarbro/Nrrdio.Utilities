using Microsoft.Extensions.Logging;
using Nrrdio.Utilities.Loggers.Contracts;
using System.Collections.Concurrent;
using System.Threading;

namespace Nrrdio.Utilities.Loggers {
    public sealed class LoggerProvider<T, TConfig> : ILoggerProvider
        where T : INrrdioLogger, new()
        where TConfig : INrrdioLoggerConfig {

        public TConfig Config { private get; init; }

        ConcurrentDictionary<string, T> Instances => new ConcurrentDictionary<string, T>();
        CancellationTokenSource CancellationTokenSource => new CancellationTokenSource();

        public ILogger CreateLogger(string categoryName) => Instances.GetOrAdd(categoryName, name => new T {
            Name = name,
            GenericConfig = Config,
            CancellationToken = CancellationTokenSource.Token
        });

        public void Dispose() {
            CancellationTokenSource.Cancel();

            foreach (var instance in Instances) {
                instance.Value.Dispose();
            }

            Instances.Clear();
        }
    }
}
