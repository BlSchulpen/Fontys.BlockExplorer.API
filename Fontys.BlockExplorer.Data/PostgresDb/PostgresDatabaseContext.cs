namespace Fontys.BlockExplorer.Data.PostgresDb
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    public class PostgresDatabaseContext : BlockExplorerContext
    {
        private readonly string _connectionString;

        public PostgresDatabaseContext(DbContextOptions options, IOptions<PostgresDbOptions> postgresDbOptions) : base(options)
        {
            _connectionString = postgresDbOptions.Value.ConnectionsString;
        }

        public PostgresDatabaseContext(IOptions<PostgresDbOptions> options)
        {
            _connectionString = options.Value.ConnectionsString;
        }

        public PostgresDatabaseContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseNpgsql(_connectionString);
        }
    }
}
