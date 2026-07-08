using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Features.Order.Models;

[Table("order")]
public sealed class OrderModel
{
    public int Id { get; set; }
    public string OrderAddress { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string OrderStatus { get; set; } = string.Empty;
}