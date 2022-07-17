namespace Fontys.BlockExplorer.Data
{
    using Fontys.BlockExplorer.Domain.Models;
    using Microsoft.EntityFrameworkCore;

    public class BlockExplorerContext: DbContext
    {
        public BlockExplorerContext(DbContextOptions options) : base(options)
        {
        }
        public BlockExplorerContext()
        {
        }

        public virtual DbSet<Block> Blocks { get; set; }
        
        public virtual DbSet<Transaction> Transactions { get; set; }

        public virtual DbSet<Transfer> Transfers { get; set; }

        public virtual DbSet<Address> Addresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            string _connectionString = "User ID=postgres;Password=Explorer;Host=localhost;Port=5432;Database=ExplorerDb;";
            dbContextOptionsBuilder.UseNpgsql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Block>()
                .HasMany(x => x.Transactions);
            
            builder.Entity<Transaction>()
                .HasMany(x => x.Transfers);
            
            builder.Entity<Transfer>()
                .HasOne(x => x.Address);
        }
    }
}