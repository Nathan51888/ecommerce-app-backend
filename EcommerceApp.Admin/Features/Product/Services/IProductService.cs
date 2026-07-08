using EcommerceApp.Features.Product.DTOs;
using EcommerceApp.Features.Product.Models;

namespace EcommerceApp.Features.Product.Services;

public interface IProductService
{
    Task<List<ProductItemModel>> GetAllAsync();
    Task<ProductItemModel?> GetByIdAsync(int id);
    Task<ProductItemModel?> CreateAsync(ProductCreateRequestDto requestDto);

    Task<ProductItemModel?>
        UpdateAsync(ProductUpdateRequestDto requestDto); //TODO: Add id argument to unit test if dto.id == provided id

    Task<ProductItemModel?> DeleteByIdAsync(int id);
}