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

        public virtual DbSet<TxInput> TxInputs { get; set; }
        public virtual DbSet<TxOutput> TxOutputs { get; set; }


        public virtual DbSet<Address> Addresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            throw new NotImplementedException();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Block>()
                .HasMany(x => x.Transactions);
            
            modelBuilder.Entity<Transaction>()
                .HasMany(x => x.Outputs);

            modelBuilder.Entity<Transaction>()
                .HasMany(x => x.Inputs);

            modelBuilder.Entity<TxInput>()
                .HasOne(x => x.Address);

            modelBuilder.Entity<TxOutput>()
                .HasOne(x => x.Address);
        }
    }
}