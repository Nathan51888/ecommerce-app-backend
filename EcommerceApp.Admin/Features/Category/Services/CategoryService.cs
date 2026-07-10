using EcommerceApp.Data;
using EcommerceApp.Features.Category.DTOs;
using EcommerceApp.Features.Category.Mappers;
using EcommerceApp.Features.Category.Models;
using Microsoft.EntityFrameworkCore;

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
        var items = await _context.Categories.ToListAsync();
        return items;
    }

    public async Task<CategoryModel?> GetByIdAsync(int id)
    {
        var itemFound = await _context.Categories.FindAsync(id);
        return itemFound;
    }

    public async Task<CategoryModel?> CreateAsync(CategoryCreateRequestDto requestDto)
    {
        var model = requestDto.ToModel();
        await _context.Categories.AddAsync(model);
        await _context.SaveChangesAsync();
        return model;
    }

    public async Task<CategoryModel?> UpdateByIdAsync(int id, CategoryUpdateRequestDto requestDto)
    {
        var itemFound = await _context.Categories.FindAsync(id);
        itemFound.Name = requestDto.Name;
        itemFound.Description = requestDto.Description;
        await _context.SaveChangesAsync();
        return itemFound;
    }

    public async Task<CategoryModel?> DeleteByIdAsync(int id)
    {
        var itemFound = await _context.Categories.FindAsync(id);
        _context.Categories.Remove(itemFound);
        await _context.SaveChangesAsync();
        return itemFound;
    }
}