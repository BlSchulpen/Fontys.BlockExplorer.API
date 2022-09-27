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

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            throw new NotImplementedException();
            //This method is empty 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Block>()
                .HasMany(x => x.Transactions);
            
            builder.Entity<Transaction>()
                .HasMany(x => x.Outputs);

            builder.Entity<Transaction>()
                .HasMany(x => x.Inputs);

            builder.Entity<TxInput>()
                .HasOne(x => x.Address);

            builder.Entity<TxOutput>()
                .HasOne(x => x.Address);
        }
    }
}