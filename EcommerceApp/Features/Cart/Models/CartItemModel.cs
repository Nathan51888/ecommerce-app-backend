using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Features.Cart.Models;

[Table("cart_item")]
public sealed class CartItemModel
{
    public int Id { get; set; }

    // FK: product
    public int ProductId { get; set; }

    public int ItemAmount { get; set; }

    // FK: customer
    public int CustomerId { get; set; }

    // public ProductItemModel Products { get; set; }
}