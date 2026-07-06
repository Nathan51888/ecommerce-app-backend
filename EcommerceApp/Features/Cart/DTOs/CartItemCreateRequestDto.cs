using EcommerceApp.Features.Products.Models;

namespace EcommerceApp.Features.Cart.DTOs;

public class CartItemCreateRequestDto
{
    // FK: products
    public int ProductsId { get; set; }
    public int ItemAmount { get; set; }
    // FK: customers
    public int CustomersId { get; set; }
}