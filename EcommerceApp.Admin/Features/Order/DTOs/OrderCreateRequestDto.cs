namespace EcommerceApp.Features.Order.DTOs;

public sealed class OrderCreateRequestDto
{
    public string OrderAddress { get; set; } = string.Empty;
    public DateTimeOffset OrderDate { get; set; }
    public enum OrderStatusEnum
    {
        Pending,
        Processing,
        Shipping,
        Delivered,
        Refunded,
        Failed,
    }

    public string OrderStatus { get; set; } = string.Empty;
}