namespace EcommerceApp.Features.Order.DTOs;

public sealed class OrderUpdateRequestDto
{
    public int Id { get; set; }
    public string OrderAddress { get; set; } = string.Empty;
    public DateTimeOffset OrderDate { get; set; }
    public string OrderStatus { get; set; } = string.Empty;
}