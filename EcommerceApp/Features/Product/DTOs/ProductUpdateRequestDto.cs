namespace EcommerceApp.Features.Products.DTOs;

public sealed class ProductUpdateRequestDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int PriceRegular { get; set; }
    public int PriceDiscount { get; set; }
    public int StockAmount { get; set; }
    public string Category { get; set; } = string.Empty;
}