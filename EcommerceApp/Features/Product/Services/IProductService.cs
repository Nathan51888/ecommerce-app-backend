using EcommerceApp.Features.Products.DTOs;
using EcommerceApp.Features.Products.Models;

namespace EcommerceApp.Features.Products.Services;

public interface IProductService
{
    Task<List<ProductItemModel?>> GetAllAsync();
    Task<ProductItemModel?> GetByIdAsync(int id);
    Task<ProductItemModel?> CreateAsync(ProductCreateRequestDto requestDto);
    Task<ProductItemModel?> UpdateAsync(ProductUpdateRequestDto requestDto);
    Task<ProductItemModel?> DeleteByIdAsync(int id);
}