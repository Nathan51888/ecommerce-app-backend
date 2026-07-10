using EcommerceApp.Features.Category.DTOs;
using EcommerceApp.Features.Category.Models;

namespace EcommerceApp.Features.Category.Services;

public interface ICategoryService
{
    Task<List<CategoryModel>> GetAllAsync();
    Task<CategoryModel?> GetByIdAsync(int id);
    Task<CategoryModel?> CreateAsync(CategoryCreateRequestDto requestDto);
    Task<CategoryModel?> UpdateByIdAsync(int id, CategoryUpdateRequestDto requestDto);
    Task<CategoryModel?> DeleteByIdAsync(int id);
}