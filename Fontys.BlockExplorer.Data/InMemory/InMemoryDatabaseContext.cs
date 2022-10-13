using Fontys.BlockExplorer.Data.PostgresDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
