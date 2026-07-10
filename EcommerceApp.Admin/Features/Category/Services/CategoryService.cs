using EcommerceApp.Data;
using EcommerceApp.Features.Category.DTOs;
using EcommerceApp.Features.Category.Models;

namespace EcommerceApp.Features.Category.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;
    
    public CategoryService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<CategoryModel>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<CategoryModel?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<CategoryModel?> CreateAsync(CategoryCreateRequestDto requestDto)
    {
        throw new NotImplementedException();
    }

    public async Task<CategoryModel?> UpdateByIdAsync(int id, CategoryUpdateRequestDto requestDto)
    {
        throw new NotImplementedException();
    }

    public async Task<CategoryModel?> DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}