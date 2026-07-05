using EcommerceApp.Features.Products.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
    }

    public DbSet<ProductItemModel> ProductItems => Set<ProductItemModel>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}