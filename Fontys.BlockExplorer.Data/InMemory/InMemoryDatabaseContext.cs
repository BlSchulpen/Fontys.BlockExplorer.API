using Microsoft.EntityFrameworkCore;

namespace Fontys.BlockExplorer.Data.InMemory
{
    public class InMemoryDatabaseContext : BlockExplorerContext
    {
        public InMemoryDatabaseContext()
        {
        }

        public InMemoryDatabaseContext(DbContextOptions<InMemoryDatabaseContext> options) : base(options)
        {

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {

        }
    }
}
