using EcommerceApp.Features.Category.DTOs;
using EcommerceApp.Features.Category.Models;

namespace EcommerceApp.Features.Category.Mappers;

public static class CategoryMapper
{
    public static CategoryModel ToModel(this CategoryCreateRequestDto dto)
    {
        return new CategoryModel
        {
            Name = dto.Name,
            Description = dto.Description
        };
    }

    public static CategoryResponseDto ToResponseDto(this CategoryModel model)
    {
        return new CategoryResponseDto
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
        };
    }
}