using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace Nrrdio.Utilities.Loggers.Contracts {
    public interface IAsyncLogger : ILogger, IDisposable {
        public string Name { init; }
        public IAsyncLoggerConfig GenericConfig { init; }
        public CancellationToken CancellationToken { init; }
    }
}
