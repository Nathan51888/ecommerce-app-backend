using EcommerceApp.Data;
using EcommerceApp.Features.Cart.DTOs;
using EcommerceApp.Features.Cart.Mappers;
using EcommerceApp.Features.Cart.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Features.Cart.Services;

public sealed class CartService : ICartService
{
    private readonly AppDbContext _context;

    public CartService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CartItemModel>> GetAllAsync(int userId)
    {
        var items = await _context.CartItems.Where(x => x.CustomerId == userId).ToListAsync();
        return items;
    }

    public async Task<CartItemModel?> GetByIdAsync(int userId, int itemId)
    {
        var item = await _context.CartItems.FindAsync(itemId);
        return item;
    }

    public async Task<CartItemModel?> CreateAsync(int userId, CartItemCreateRequestDto requestDto)
    {
        var model = requestDto.ToModel();
        model.CustomerId = userId;
        var createdItem = await _context.CartItems.AddAsync(model);
        await _context.SaveChangesAsync();

        return createdItem.Entity;
    }

    public async Task<CartItemModel?> UpdateByIdAsync(int userId, CartItemUpdateRequestDto requestDto)
    {
        var existingItem =
            await _context.CartItems.FirstOrDefaultAsync(x => x.CustomerId == userId && x.Id == requestDto.Id);
        // if (existingItem == null)
        //     return null;

        existingItem.ProductId = requestDto.ProductId;
        existingItem.ItemAmount = requestDto.ItemAmount;
        await _context.SaveChangesAsync();

        var updatedItem =
            await _context.CartItems.FirstOrDefaultAsync(x => x.CustomerId == userId && x.Id == requestDto.Id);
        return updatedItem;
    }

    public async Task<CartItemModel?> DeleteByIdAsync(int userId, int itemId)
    {
        var existingItem = await _context.CartItems.FirstOrDefaultAsync(x => x.CustomerId == userId && x.Id == itemId);
        if (existingItem == null)
            return null;

        _context.Remove(existingItem);
        await _context.SaveChangesAsync();

        return existingItem;
    }
}