using EcommerceApp.Features.Order.DTOs;
using EcommerceApp.Features.Order.Models;

namespace EcommerceApp.Features.Order.Services;

public interface IOrderService
{
    Task<List<OrderModel?>> GetAllAsync();
    Task<OrderModel?> GetByIdAsync(int id);
    Task<OrderModel?> CreateAsync(OrderCreateRequestDto requestDto);
    Task<OrderModel?> UpdateByIdAsync(int id, OrderUpdateRequestDto requestDto);
    Task<OrderModel?> DeleteByIdAsync(int id);
}