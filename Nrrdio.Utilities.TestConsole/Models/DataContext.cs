using Nrrdio.Utilities.Loggers;
using Microsoft.EntityFrameworkCore;

namespace Nrrdio.Utilities.TestConsole.Models {
    public class DataContext : DbContext {
        public DbSet<LogEntry> LogEntries { get; set; }

        public DataContext() {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite("Data Source=AppData.db");
        }
    }
}
