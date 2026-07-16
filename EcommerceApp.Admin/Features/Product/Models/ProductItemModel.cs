using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Features.Product.Models;

[Table("product")]
public sealed class ProductItemModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int PriceRegular { get; set; }
    public int PriceDiscount { get; set; }
    public int StockAmount { get; set; }

    public string Category { get; set; } = string.Empty;
    // FK: category
    // public int CategoryId { get; set; }
    // public CategoryModel Category { get; set; }
}