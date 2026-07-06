using EcommerceApp.Data;
using EcommerceApp.Features.Cart.DTOs;
using EcommerceApp.Features.Cart.Models;

namespace EcommerceApp.Features.Cart.Services;

public class CartService : ICartService
{
    private readonly AppDbContext _context;

    public CartService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<CartItemModel>> GetAllAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public async Task<CartItemModel?> GetByIdAsync(int userId, int itemId)
    {
        throw new NotImplementedException();
    }

    public async Task<CartItemModel?> CreateAsync(int userId, CartItemCreateRequestDto requestDto)
    {
        throw new NotImplementedException();
    }

    public async Task<CartItemModel?> UpdateByIdAsync(int userId, CartItemUpdateRequestDto requestDto)
    {
        throw new NotImplementedException();
    }

    public async Task<CartItemModel?> DeleteByIdAsync(int userId, int itemId)
    {
        throw new NotImplementedException();
    }
}