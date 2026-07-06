using Bogus;
using EcommerceApp.Data;
using EcommerceApp.Features.Cart.DTOs;
using EcommerceApp.Features.Cart.Models;
using EcommerceApp.Features.Cart.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Test.Features.Cart.Services;

public class CartServiceTest
{
    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("testing")
            .Options;
        
        return new AppDbContext(options);
    }
    
    private readonly AppDbContext _context;
    private readonly ICartService _sut;

    private readonly Faker<CartItemModel> _cartItemGenerator =
        new Faker<CartItemModel>()
            .RuleFor(x => x.ItemAmount, f => f.Random.Number())
            .UseSeed(65465);

    public CartServiceTest()
    {
        _context = CreateDbContext();
        _sut = new CartService(_context);
    }

    [Fact]
    public async Task GetAll()
    {
        // Arrange
        var userId = 3;

        var toCreateItemList = new List<CartItemModel>()
        {
            _cartItemGenerator.Generate(),
            _cartItemGenerator.Generate(),
            _cartItemGenerator.Generate(),
            _cartItemGenerator.Generate(),
        };
        var createdItemList = new List<CartItemModel>();
        foreach (var item in toCreateItemList)
        {
            var dto = new CartItemCreateRequestDto
            {
                ProductsId = item.ProductsId,
                ItemAmount = item.ItemAmount,
                CustomersId = userId,
            };
            var createdItem = await _sut.CreateAsync(userId, dto);
            createdItem.Should().NotBeNull();
            createdItem.Should().BeEquivalentTo(dto);
            createdItemList.Add(createdItem);
        }
        
        // Act
        var items = await _sut.GetAllAsync(userId);

        // Assert
        var expected = createdItemList;
        items.Should().NotBeNull();
        items.Should().NotBeEmpty();
        items.Should().Contain(expected);
    }

    [Fact]
    public async Task GetById()
    {
        // Arrange
        var userId = 3;
        var createModel = _cartItemGenerator.UseSeed(654).Generate();
        var createDto = new CartItemCreateRequestDto
        {
            ProductsId = createModel.ProductsId,
            ItemAmount = createModel.ItemAmount,
            CustomersId = userId
        };
        var createdItem = await _sut.CreateAsync(userId, createDto);
        createdItem.Should().NotBeNull();
        createdItem.Should().BeEquivalentTo(createDto);

        // Act
        var item = await _sut.GetByIdAsync(userId, createdItem.Id);

        // Assert
        var expected = createdItem;
        item.Should().NotBeNull();
        item.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Create()
    {
        // Arrange
        var userId = 3;
        var cartItem = _cartItemGenerator.Generate();

        // Act
        var dto = new CartItemCreateRequestDto
        {
            ProductsId = cartItem.ProductsId,
            ItemAmount = cartItem.ItemAmount,
            CustomersId = userId,
        };
        var createdItem = await _sut.CreateAsync(userId, dto);

        // Assert
        var expected = dto;
        createdItem.Should().NotBeNull();
        createdItem.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task UpdateById()
    {
        // Arrange
        var userId = 3;
        var createDto = new CartItemCreateRequestDto
        {
            ProductsId = 5,
            ItemAmount = 1,
            CustomersId = userId
        };
        var createdItem = await _sut.CreateAsync(userId, createDto);
        createdItem.Should().NotBeNull();
        createdItem.Should().BeEquivalentTo(createDto);

        // Act
        var dto = new CartItemUpdateRequestDto
        {
            Id = createdItem.Id,
            ProductsId = 6,
            ItemAmount = 1,
            CustomersId = userId
        };
        var updatedItem = await _sut.UpdateByIdAsync(userId, dto);

        // Assert
        var expected = dto;
        updatedItem.Should().NotBeNull();
        updatedItem.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task DeleteById()
    {
        // Arrange
        var userId = 3;
        var createModel = _cartItemGenerator.UseSeed(654).Generate();
        var createDto = new CartItemCreateRequestDto
        {
            ProductsId = createModel.ProductsId,
            ItemAmount = createModel.ItemAmount,
            CustomersId = userId
        };
        var createdItem = await _sut.CreateAsync(userId, createDto);
        createdItem.Should().NotBeNull();
        createdItem.Should().BeEquivalentTo(createDto);

        // Act
        var deletedItem = await _sut.DeleteByIdAsync(userId, createdItem.Id);

        // Assert
        var expected = createdItem;
        deletedItem.Should().NotBeNull();
        deletedItem.Should().BeEquivalentTo(expected);
    }
}