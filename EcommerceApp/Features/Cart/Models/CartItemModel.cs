using System.ComponentModel.DataAnnotations.Schema;
using EcommerceApp.Features.Products.Models;

namespace EcommerceApp.Features.Cart.Models;

[Table("carts_items")]
public sealed class CartItemModel
{
    public int Id { get; set; }
    // FK: products
    public int ProductsId { get; set; }
    public int ItemAmount { get; set; }
    // FK: customers
    public int CustomersId { get; set; }
    
    // public ProductItemModel Products { get; set; }
}