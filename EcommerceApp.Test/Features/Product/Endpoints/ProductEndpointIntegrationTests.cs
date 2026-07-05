using System.Net;
using System.Net.Http.Json;
using Bogus;
using EcommerceApp.Features.Products.DTOs;
using EcommerceApp.Features.Products.Models;
using EcommerceApp.Test.Abstractions;
using FluentAssertions;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using Xunit.Abstractions;

namespace EcommerceApp.Test.Features.Product.Endpoints;

public class ProductEndpointIntegrationTests : BaseIntegrationTest
{
    public ProductEndpointIntegrationTests(IntegrationTestWebAppFactory factory, ITestOutputHelper testOutputHelper) : base(factory, testOutputHelper)
    {
    }

    private readonly Faker<ProductItemModel> _productGenerator =
        new Faker<ProductItemModel>()
            .RuleFor(x => x.Name, f => f.Commerce.ProductName())
            .RuleFor(x => x.Description, f => f.Lorem.Paragraph())
            .RuleFor(x => x.Category, f => f.Lorem.Word())
            .RuleFor(x => x.PriceRegular, f => f.Random.Number())
            .RuleFor(x => x.PriceDiscount, f => f.Random.Number())
            .RuleFor(x => x.StockAmount, f => f.Random.Number())
            .UseSeed(1000);

    [Fact]
    public async Task GetAll()
    {
        // Arrange
        var expected = new List<ProductItemModel>()
        {
            new ProductItemModel
            {
                Id = 0,
                Name = "name1",
                Description = "description1",
                PriceRegular = 0,
                PriceDiscount = 0,
                StockAmount = 0,
                Category = "cat1"
            }
        };
        
        // Act
        var res = await HttpClient.GetAsync(ProductConstants.ProductEndpoint);

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task GetById()
    {
        // Arrange
        var id = 2;
        
        // Act
        var res = await HttpClient.GetAsync($"{ProductConstants.ProductEndpoint}/{2}");

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Create()
    {
        // Arrange
        var postReq = _productGenerator.Generate();
        var expected = new ProductResponseDto()
        {
            Id = 0,
            Name = postReq.Name,
            Description = postReq.Description,
            PriceRegular = postReq.PriceRegular,
            PriceDiscount = postReq.PriceDiscount,
            StockAmount = postReq.StockAmount,
            Category = postReq.Category,
        };
        
        // Act
        var res = await HttpClient.PostAsJsonAsync(ProductConstants.ProductEndpoint, postReq);
        var resContent = await res.Content.ReadFromJsonAsync<ProductResponseDto>();

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.Created);
        resContent.Should().BeEquivalentTo(expected, opt => opt.Excluding(x => x.Id));
    }
    
    [Fact]
    public async Task UpdateById()
    {
        // Arrange
        var id = 2;
        var putReq = new ProductUpdateRequestDto
        {
            Id = 2,
            Name = "updatedName",
            Description = "updatedDescription",
            PriceRegular = 0,
            PriceDiscount = 0,
            StockAmount = 0,
            Category = "updatedCategory"
        };
        
        // Act
        var res = await HttpClient.PutAsJsonAsync($"{ProductConstants.ProductEndpoint}/{id}", putReq);

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task DeleteById()
    {
        // Arrange
        var id = 2;
        
        // Act
        var res = await HttpClient.DeleteAsync($"{ProductConstants.ProductEndpoint}/{id}");

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}