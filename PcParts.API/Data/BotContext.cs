using Microsoft.EntityFrameworkCore;
using PcParts.API.Models;

namespace PcParts.API.Data;

public class BotContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Reason> Reasons { get; set; }
    public DbSet<ReturnOrder> ReturnOrders { get; set; }

    public BotContext(DbContextOptions<BotContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().ToTable("Product");
        modelBuilder.Entity<Order>().ToTable("Order");
        modelBuilder.Entity<Reason>().ToTable("Reason");
        modelBuilder.Entity<ReturnOrder>().ToTable("ReturnOrder");

        base.OnModelCreating(modelBuilder);
    }
}