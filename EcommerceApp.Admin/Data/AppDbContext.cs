using EcommerceApp.Features.Cart.Models;
using EcommerceApp.Features.Category.Models;
using EcommerceApp.Features.Order.Models;
using EcommerceApp.Features.Product.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EcommerceApp.Data;

public sealed class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
    }

    public DbSet<ProductItemModel> ProductItems => Set<ProductItemModel>();
    public DbSet<CategoryModel> Categories => Set<CategoryModel>();
    public DbSet<OrderModel> Orders => Set<OrderModel>();
    public DbSet<CartItemModel> CartItems => Set<CartItemModel>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var properties = entityType.GetProperties()
                .Where(p => p.ClrType == typeof(DateTimeOffset) || p.ClrType == typeof(DateTimeOffset?));

            foreach (var property in properties)
                // Syncs .NET precision matching Postgres's 6-digit microsecond ceiling
                property.SetPrecision(6);
        }

        // builder.Entity<ApplicationUser>(entity =>
        // {
        //     entity.Property(e => e.EnableNotifications).HasDefaultValue(true);
        //     entity.Property(e => e.Initials).HasMaxLength(5);
        // });
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<DateTimeOffset>()
            .HaveConversion<DateTimeOffsetUtcConverter>();

        configurationBuilder
            .Properties<DateTimeOffset?>()
            .HaveConversion<NullableDateTimeOffsetUtcConverter>();
    }

    // Custom converters to enforce UTC
    private class DateTimeOffsetUtcConverter : ValueConverter<DateTimeOffset, DateTimeOffset>
    {
        public DateTimeOffsetUtcConverter()
            : base(c => c.ToUniversalTime(), c => c.ToUniversalTime())
        {
        }
    }

    private class NullableDateTimeOffsetUtcConverter : ValueConverter<DateTimeOffset?, DateTimeOffset?>
    {
        public NullableDateTimeOffsetUtcConverter()
            : base(c => c.HasValue ? c.Value.ToUniversalTime() : c,
                c => c.HasValue ? c.Value.ToUniversalTime() : c)
        {
        }
    }
}