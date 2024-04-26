using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Monopoly.Models;

public sealed class WarehouseContext : DbContext
{
    public WarehouseContext()
    { 
        Database.EnsureCreated();
    }
    
    public WarehouseContext(DbContextOptions<WarehouseContext> options) : base(options)
    { 
        Database.EnsureCreated();
    }
    
    public DbSet<Box> Boxes { get; set; } = null!;
    public DbSet<Pallet> Pallets { get; set; } = null!;
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json").Build();
        var connectionString = config.GetConnectionString("Postgres");
        optionsBuilder.UseNpgsql(connectionString);
    }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}