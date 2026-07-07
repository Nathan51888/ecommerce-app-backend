using EcommerceApp.Features.Product.DTOs;
using EcommerceApp.Features.Product.Models;

namespace EcommerceApp.Features.Product.Mappers;

public static class ProductMapper
{
    public static ProductItemModel ToModel(this ProductCreateRequestDto dto)
    {
        return new ProductItemModel
        {
            Name = dto.Name,
            Description = dto.Description,
            PriceRegular = dto.PriceRegular,
            PriceDiscount = dto.PriceDiscount,
            StockAmount = dto.StockAmount,
            Category = dto.Category
        };
    }

    public static ProductResponseDto ToResponseDto(this ProductItemModel model)
    {
        return new ProductResponseDto
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            PriceRegular = model.PriceRegular,
            PriceDiscount = model.PriceDiscount,
            StockAmount = model.StockAmount,
            Category = model.Category
        };
    }
}