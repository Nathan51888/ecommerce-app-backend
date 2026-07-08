using Bogus;
using EcommerceApp.Data;
using EcommerceApp.Features.Order.DTOs;
using EcommerceApp.Features.Order.Models;
using EcommerceApp.Features.Order.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Admin.Test.Features.Order.Services;

public sealed class OrderServiceTest
{
    private readonly AppDbContext _context;

    private readonly Faker<OrderModel> _orderGenerator =
        new Faker<OrderModel>()
            .RuleFor(x => x.OrderDate, f => f.Date.Soon())
            .RuleFor(x => x.OrderStatus, f => f.Lorem.Word())
            .RuleFor(x => x.OrderAddress, f => f.Address.FullAddress())
            .UseSeed(3210);

    private readonly IOrderService _sut;

    public OrderServiceTest()
    {
        _context = CreateDbContext();
        _sut = new OrderService(_context);
    }

    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("testing")
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetAll()
    {
        // Arrange
        var toCreateItemList = new List<OrderModel>
        {
            _orderGenerator.Generate(),
            _orderGenerator.Generate(),
            _orderGenerator.Generate(),
            _orderGenerator.Generate()
        };
        var createdItemList = new List<OrderModel>();
        foreach (var order in toCreateItemList)
        {
            var dto = new OrderCreateRequestDto
            {
                OrderAddress = order.OrderAddress,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus
            };
            var createdItem = await _sut.CreateAsync(dto);
            createdItem.Should().NotBeNull();
            createdItem.Should().BeEquivalentTo(order, opt => opt.Excluding(x => x.Id));
            createdItemList.Add(createdItem);
        }

        // Act
        var itemList = await _sut.GetAllAsync();

        // Assert
        var expected = createdItemList;
        itemList.Should().NotBeNull();
        itemList.Should().Contain(expected);
    }

    [Fact]
    public async Task GetById()
    {
        // Arrange
        var createModel = _orderGenerator.UseSeed(654).Generate();
        var createDto = new OrderCreateRequestDto
        {
            OrderAddress = createModel.OrderAddress,
            OrderDate = createModel.OrderDate,
            OrderStatus = createModel.OrderStatus
        };
        var createdItem = await _sut.CreateAsync(createDto);
        createdItem.Should().NotBeNull();
        createdItem.Should().BeEquivalentTo(createModel, opt => opt.Excluding(x => x.Id));

        // Act
        var item = await _sut.GetByIdAsync(createdItem.Id);

        // Assert
        item.Should().NotBeNull();
        item.Should().BeEquivalentTo(createdItem);
    }

    [Fact]
    public async Task Create()
    {
        // Arrange
        var order = _orderGenerator.Generate();

        // Act
        var dto = new OrderCreateRequestDto
        {
            OrderAddress = order.OrderAddress,
            OrderDate = order.OrderDate,
            OrderStatus = order.OrderStatus
        };
        var createdItem = await _sut.CreateAsync(dto);

        // Assert
        createdItem.Should().NotBeNull();
        createdItem.Should().BeEquivalentTo(order, opt => opt.Excluding(x => x.Id));
    }

    [Fact]
    public async Task UpdateById()
    {
        // Arrange
        var createModel = _orderGenerator.UseSeed(654).Generate();
        var createDto = new OrderCreateRequestDto
        {
            OrderAddress = createModel.OrderAddress,
            OrderDate = createModel.OrderDate,
            OrderStatus = createModel.OrderStatus
        };
        var createdItem = await _sut.CreateAsync(createDto);
        createdItem.Should().NotBeNull();
        createdItem.Should().BeEquivalentTo(createModel, opt => opt.Excluding(x => x.Id));

        // Act
        var dto = new OrderUpdateRequestDto
        {
            Id = createdItem.Id,
            OrderAddress = "updated",
            OrderDate = DateTime.Now,
            OrderStatus = "updated"
        };
        var updatedItem = await _sut.UpdateByIdAsync(dto.Id, dto);

        // Assert
        updatedItem.Should().NotBeNull();
        updatedItem.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task DeleteById()
    {
        // Arrange
        var createModel = _orderGenerator.UseSeed(654).Generate();
        var createDto = new OrderCreateRequestDto
        {
            OrderAddress = createModel.OrderAddress,
            OrderDate = createModel.OrderDate,
            OrderStatus = createModel.OrderStatus
        };
        var createdItem = await _sut.CreateAsync(createDto);
        createdItem.Should().NotBeNull();
        createdItem.Should().BeEquivalentTo(createModel, opt => opt.Excluding(x => x.Id));

        // Act
        var deletedItem = await _sut.DeleteByIdAsync(createdItem.Id);

        // Assert
        deletedItem.Should().NotBeNull();
        deletedItem.Should().BeEquivalentTo(createdItem);
    }
}