using Microsoft.EntityFrameworkCore;
using ReadFile_Mini.Models;

namespace ReadFile_Mini.Context
{
    public class PostgreSqlDbContext : DbContext
    {
        public PostgreSqlDbContext(DbContextOptions<PostgreSqlDbContext> options) : base(options) { }

        public DbSet<DestinationData> destinationdata { get; set; }
        public DbSet<InventoryDestination> inventorydestinations { get; set; }

       
    }

    
}
