namespace EcommerceApp.Features.Cart.DTOs;

public class CartItemResponseDto
{
    public int Id { get; set; }
    // FK: products
    public int ProductsId { get; set; }
    public int ItemAmount { get; set; }
    // FK: customers
    public int CustomersId { get; set; }
}