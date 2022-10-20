using Microsoft.EntityFrameworkCore;

namespace Fontys.BlockExplorer.Data.InMemory
{
    public class InMemoryDatabaseContext : BlockExplorerContext
    {
        private readonly string _connectionString;

        public InMemoryDatabaseContext()
        {
        }

        public InMemoryDatabaseContext(DbContextOptions<InMemoryDatabaseContext> options) : base(options)
        {
            _connectionString = "";
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            //  optionBuilder.UseNpgsql(_connectionString);
        }
    }
}
