namespace Fontys.BlockExplorer.Data.PostgresDb
{
    using Microsoft.EntityFrameworkCore;

    public class PostgresDatabaseContext : BlockExplorerContext
    {
        private readonly string _connectionString;
        public PostgresDatabaseContext(DbContextOptions<BlockExplorerContext> options, PostgresDbOptions postgresDbOptions) : base(options)
        {
            _connectionString = postgresDbOptions.ConnectionsString;
        }

        public PostgresDatabaseContext()
        {
        }

        protected void OnConfigure(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.UseNpgsql(_connectionString);

        }
    }
}
