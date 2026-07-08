using EcommerceApp.Features.Cart.DTOs;
using EcommerceApp.Features.Cart.Models;

namespace EcommerceApp.Features.Cart.Mappers;

public static class CartMapper
{
    public static CartItemModel ToModel(this CartItemCreateRequestDto dto)
    {
        return new CartItemModel
        {
            ProductId = dto.ProductId,
            ItemAmount = dto.ItemAmount,
            CustomerId = dto.CustomerId
        };
    }

    public static CartItemResponseDto ToResponseDto(this CartItemModel model)
    {
        return new CartItemResponseDto
        {
            Id = model.Id,
            ProductId = model.ProductId,
            ItemAmount = model.ItemAmount,
            CustomerId = model.CustomerId
        };
    }
}