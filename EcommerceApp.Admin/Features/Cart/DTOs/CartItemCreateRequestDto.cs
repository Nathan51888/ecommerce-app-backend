namespace EcommerceApp.Features.Cart.DTOs;

public sealed class CartItemCreateRequestDto
{
    // FK: product
    public int ProductId { get; set; }

    public int ItemAmount { get; set; }

    // FK: customer
    public int CustomerId { get; set; }
}