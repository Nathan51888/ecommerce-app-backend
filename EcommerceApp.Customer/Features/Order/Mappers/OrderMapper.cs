using EcommerceApp.Features.Order.DTOs;
using EcommerceApp.Features.Order.Models;

namespace EcommerceApp.Features.Order.Mappers;

public static class OrderMapper
{
    public static OrderResponseDto ToResponseDto(this OrderModel model)
    {
        return new OrderResponseDto
        {
            Id = model.Id,
            OrderAddress = model.OrderAddress,
            OrderDate = model.OrderDate,
            OrderStatus = model.OrderStatus
        };
    }

    public static OrderModel ToModel(this OrderCreateRequestDto dto)
    {
        return new OrderModel
        {
            OrderAddress = dto.OrderAddress,
            OrderDate = dto.OrderDate,
            OrderStatus = dto.OrderStatus
        };
    }
}