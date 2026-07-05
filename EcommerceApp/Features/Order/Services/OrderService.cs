using EcommerceApp.Data;
using EcommerceApp.Features.Order.DTOs;
using EcommerceApp.Features.Order.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Features.Order.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrderModel?>> GetAllAsync()
    {
        var items = await _context.Orders.ToListAsync();
        return items;
    }

    public async Task<OrderModel?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<OrderModel?> CreateAsync(OrderCreateRequestDto requestDto)
    {
        throw new NotImplementedException();
    }

    public async Task<OrderModel?> UpdateByIdAsync(int id, OrderUpdateRequestDto requestDto)
    {
        throw new NotImplementedException();
    }

    public async Task<OrderModel?> DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}