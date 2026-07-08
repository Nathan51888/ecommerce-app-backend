using EcommerceApp.Data;
using EcommerceApp.Features.Order.DTOs;
using EcommerceApp.Features.Order.Mappers;
using EcommerceApp.Features.Order.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Features.Order.Services;

public sealed class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrderModel>> GetAllAsync()
    {
        var items = await _context.Orders.ToListAsync();
        return items;
    }

    public async Task<OrderModel?> GetByIdAsync(int id)
    {
        var item = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
        return item;
    }

    public async Task<OrderModel?> CreateAsync(OrderCreateRequestDto requestDto)
    {
        var itemModel = requestDto.ToModel();
        var created = await _context.Orders.AddAsync(itemModel);
        await _context.SaveChangesAsync();

        return created.Entity;
    }

    public async Task<OrderModel?> UpdateByIdAsync(int id, OrderUpdateRequestDto requestDto)
    {
        var existingItem = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
        existingItem?.OrderDate = requestDto.OrderDate;
        existingItem?.OrderStatus = requestDto.OrderStatus;
        existingItem?.OrderAddress = requestDto.OrderAddress;
        await _context.SaveChangesAsync();
        return existingItem;
    }

    public async Task<OrderModel?> DeleteByIdAsync(int id)
    {
        var existingItem = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
        _context.Orders.Remove(existingItem);
        await _context.SaveChangesAsync();
        return existingItem;
    }
}