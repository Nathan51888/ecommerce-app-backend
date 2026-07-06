using EcommerceApp.Features.Cart.DTOs;
using EcommerceApp.Features.Cart.Models;

namespace EcommerceApp.Features.Cart.Mappers;

public static class CartMapper
{
    public static CartItemModel ToModel(this CartItemCreateRequestDto dto)
    {
        return new CartItemModel
        {
            ProductsId = dto.ProductsId,
            ItemAmount = dto.ItemAmount,
            CustomersId = dto.CustomersId,
        };
    }

    public static CartItemResponseDto ToResponseDto(this CartItemModel model)
    {
        return new CartItemResponseDto
        {
            Id = model.Id,
            ProductsId = model.ProductsId,
            ItemAmount = model.ItemAmount,
        };
    }
}