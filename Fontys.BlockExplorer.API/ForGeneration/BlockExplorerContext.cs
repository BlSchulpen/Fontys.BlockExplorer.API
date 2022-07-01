namespace Fontys.BlockExplorer.API.ForGeneration
{
    using Fontys.BlockExplorer.Domain.Models;
    using Microsoft.EntityFrameworkCore;

    public class BlockExplorerContext : DbContext
    {
        /*
        public BlockExplorerContext(DbContextOptions options) : base(options)
        {
        }
        */

        public BlockExplorerContext(DbContextOptions<BlockExplorerContext> options) : base(options)
        {
        }

        //public virtual DbSet<Address> Addresss { get; set; }
    //    public virtual DbSet<Block> Blocks { get; set; }
  //      public virtual DbSet<Transaction> Transactions { get; set; }

  //      public virtual DbSet<Transfer> Transfers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /*builder.Entity<Address>()

               .HasIndex(x => x.Hash)
               .IsUnique();
           builder.Entity<Block>()
               .HasIndex(x => x.Hash)
               .IsUnique();
           builder.Entity<Transaction>()
               .HasIndex(x => x.Hash)
               .IsUnique();
           builder.Entity<Transfer>()
               .HasIndex(x => x.Id)
               .IsUnique();*/
        }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            string _connectionString = "User ID=postgres;Password=Explorer;Host=localhost;Port=5432;Database=ExplorerDb;";
            dbContextOptionsBuilder.UseNpgsql(_connectionString);
        }

    }
}