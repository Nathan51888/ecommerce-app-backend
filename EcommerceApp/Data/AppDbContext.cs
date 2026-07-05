using EcommerceApp.Features.Order.Models;
using EcommerceApp.Features.Products.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
    }

    public DbSet<ProductItemModel> ProductItems => Set<ProductItemModel>();
    public DbSet<OrderModel> Orders => Set<OrderModel>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}