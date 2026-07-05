using EcommerceApp.Data;
using EcommerceApp.Features.Product.Mappers;
using EcommerceApp.Features.Products.DTOs;
using EcommerceApp.Features.Products.Models;
using EcommerceApp.Features.Products.Services;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Features.Product.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;
    public ProductService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<ProductItemModel>> GetAllAsync()
    {
        var items = await _context.ProductItems.ToListAsync();
        return items;
    }

    public async Task<ProductItemModel?> GetByIdAsync(int id)
    {
        var item = await _context.ProductItems.FirstOrDefaultAsync(item => item.Id == id);
        return item;
    }

    public async Task<ProductItemModel?> CreateAsync(ProductCreateRequestDto requestDto)
    {
        var itemModel = requestDto.ToModel();
        await _context.ProductItems.AddAsync(itemModel);
        await _context.SaveChangesAsync();
        return itemModel;
    }

    public async Task<ProductItemModel?> UpdateAsync(ProductUpdateRequestDto requestDto)
    {
        throw new NotImplementedException();
    }

    public async Task<ProductItemModel?> DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}