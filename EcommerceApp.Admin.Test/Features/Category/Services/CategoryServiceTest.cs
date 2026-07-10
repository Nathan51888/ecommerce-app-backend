using Bogus;
using EcommerceApp.Data;
using EcommerceApp.Features.Category.DTOs;
using EcommerceApp.Features.Category.Models;
using EcommerceApp.Features.Category.Services;
using EcommerceApp.Features.Order.DTOs;
using EcommerceApp.Features.Order.Models;
using EcommerceApp.Features.Order.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Admin.Test.Features.Category.Services;

public sealed class CategoryServiceTest
{
    private readonly AppDbContext _context;

    private readonly Faker<CategoryModel> _orderGenerator =
        new Faker<CategoryModel>()
            .RuleFor(x => x.Name, f => f.Random.Word())
            .RuleFor(x => x.Description, f => f.Random.Words())
            .UseSeed(3210);

    private readonly ICategoryService _sut;

    public CategoryServiceTest()
    {
        _context = CreateDbContext();
        _sut = new CategoryService(_context);
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
        var toCreateItemList = new List<CategoryModel>
        {
            _orderGenerator.Generate(),
            _orderGenerator.Generate(),
            _orderGenerator.Generate(),
            _orderGenerator.Generate()
        };
        var createdItemList = new List<CategoryModel>();
        foreach (var order in toCreateItemList)
        {
            var dto = new CategoryCreateRequestDto
            {
                Name = order.Name,
                Description = order.Description
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
        var createdItem = await CreateCategory();

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
        var dto = new CategoryCreateRequestDto
        {
            Name = order.Name,
            Description = order.Description
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
        var createdItem = await CreateCategory();

        // Act
        var dto = new CategoryUpdateRequestDto
        {
            Name = "updated",
            Description = "updated",
        };
        var updatedItem = await _sut.UpdateByIdAsync(createdItem.Id, dto);

        // Assert
        updatedItem.Should().NotBeNull();
        updatedItem.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task DeleteById()
    {
        // Arrange
        var createdItem = await CreateCategory();

        // Act
        var deletedItem = await _sut.DeleteByIdAsync(createdItem.Id);

        // Assert
        deletedItem.Should().NotBeNull();
        deletedItem.Should().BeEquivalentTo(createdItem);
    }

    private async Task<CategoryModel> CreateCategory()
    {
        var createModel = _orderGenerator.UseSeed(654).Generate();
        var createDto = new CategoryCreateRequestDto
        {
            Name = createModel.Name,
            Description = createModel.Description
        };
        var createdItem = await _sut.CreateAsync(createDto);
        createdItem.Should().NotBeNull();
        createdItem.Should().BeEquivalentTo(createModel, opt => opt.Excluding(x => x.Id));
        return createdItem;
    }
}