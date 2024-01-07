using Nrrdio.Utilities.Loggers;
using Nrrdio.Utilities.Loggers.Contracts;
using Nrrdio.Utilities.TestConsole.Models;

namespace Nrrdio.Utilities.TestConsole.Utilities; 
class LogEntryTestRepository : ILogEntryRepository {
    DataContext Db { get; init; }

    public LogEntryTestRepository(
        DataContext db
    ) {
        Db = db;
    }

    public void Add(LogEntry logEntry) {
        Db.Add(logEntry);
        Db.SaveChanges();
    }
}
