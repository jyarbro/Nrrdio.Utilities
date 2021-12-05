using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace Nrrdio.Utilities.Loggers.Contracts {
    public interface INrrdioLogger : ILogger, IDisposable {
        public string Name { init; }
        public INrrdioLoggerConfig GenericConfig { init; }
        public CancellationToken CancellationToken { init; }
    }
}
