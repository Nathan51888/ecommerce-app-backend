using EcommerceApp.Features.Cart.DTOs;
using EcommerceApp.Features.Cart.Models;

namespace EcommerceApp.Features.Cart.Services;

public interface ICartService
{
    Task<List<CartItemModel>> GetAllAsync(int userId);
    Task<CartItemModel?> GetByIdAsync(int userId, int itemId);
    Task<CartItemModel?> CreateAsync(int userId, CartItemCreateRequestDto requestDto);
    Task<CartItemModel?> UpdateByIdAsync(int userId, CartItemUpdateRequestDto requestDto);
    Task<CartItemModel?> DeleteByIdAsync(int userId, int itemId);
}