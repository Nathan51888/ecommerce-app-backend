using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Features.Order.Models;

[Table("order")]
public sealed class OrderModel
{
    public int Id { get; set; }

    // FK: customer
    // public string CustomerId { get; set; } = string.Empty;
    public string OrderAddress { get; set; } = string.Empty;

    public DateTimeOffset OrderDate { get; set; }

    public string OrderStatus { get; set; } = string.Empty;
}