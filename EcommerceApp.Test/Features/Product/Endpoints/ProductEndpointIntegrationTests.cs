using System.Net;
using System.Net.Http.Json;
using Bogus;
using EcommerceApp.Features.Products.DTOs;
using EcommerceApp.Features.Products.Models;
using EcommerceApp.Test.Abstractions;
using FluentAssertions;
using Xunit.Abstractions;

namespace EcommerceApp.Test.Features.Product.Endpoints;

public class ProductEndpointIntegrationTests : BaseIntegrationTest
{
    private readonly Faker<ProductItemModel> _productGenerator =
        new Faker<ProductItemModel>()
            .RuleFor(x => x.Name, f => f.Commerce.ProductName())
            .RuleFor(x => x.Description, f => f.Lorem.Paragraph())
            .RuleFor(x => x.Category, f => f.Lorem.Word())
            .RuleFor(x => x.PriceRegular, f => f.Random.Number())
            .RuleFor(x => x.PriceDiscount, f => f.Random.Number())
            .RuleFor(x => x.StockAmount, f => f.Random.Number())
            .UseSeed(1000);

    public ProductEndpointIntegrationTests(IntegrationTestWebAppFactory factory, ITestOutputHelper testOutputHelper) : base(factory, testOutputHelper)
    {
    }

    [Fact]
    public async Task Create()
    {
        // Arrange
        var postReq = _productGenerator.Generate();
        var expected = new ProductResponseDto
        {
            Id = 0,
            Name = postReq.Name,
            Description = postReq.Description,
            PriceRegular = postReq.PriceRegular,
            PriceDiscount = postReq.PriceDiscount,
            StockAmount = postReq.StockAmount,
            Category = postReq.Category
        };

        // Act
        var res = await HttpClient.PostAsJsonAsync(ProductConstants.ProductEndpoint, postReq);
        var resContent = await res.Content.ReadFromJsonAsync<ProductResponseDto>();

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.Created);
        resContent.Should().BeEquivalentTo(expected, opt => opt.Excluding(x => x.Id));
    }

    [Fact]
    public async Task GetById()
    {
        // Arrange
        var postReq = _productGenerator.Generate();
        var expected = new ProductResponseDto
        {
            Id = 0,
            Name = postReq.Name,
            Description = postReq.Description,
            PriceRegular = postReq.PriceRegular,
            PriceDiscount = postReq.PriceDiscount,
            StockAmount = postReq.StockAmount,
            Category = postReq.Category
        };

        var postRes = await HttpClient.PostAsJsonAsync(ProductConstants.ProductEndpoint, postReq);
        var postResContent = await postRes.Content.ReadFromJsonAsync<ProductResponseDto>();

        postRes.StatusCode.Should().Be(HttpStatusCode.Created);
        postResContent.Should().BeEquivalentTo(expected, opt => opt.Excluding(x => x.Id));
        expected.Id = postResContent.Id;


        // Act
        var res = await HttpClient.GetAsync($"{ProductConstants.ProductEndpoint}/{expected.Id}");
        var resContent = await res.Content.ReadFromJsonAsync<ProductResponseDto>();

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        resContent.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task UpdateById()
    {
        // Arrange
        var postReq = _productGenerator.Generate();
        var postReqExpected = new ProductResponseDto
        {
            Id = 0,
            Name = postReq.Name,
            Description = postReq.Description,
            PriceRegular = postReq.PriceRegular,
            PriceDiscount = postReq.PriceDiscount,
            StockAmount = postReq.StockAmount,
            Category = postReq.Category
        };

        var postRes = await HttpClient.PostAsJsonAsync(ProductConstants.ProductEndpoint, postReq);
        var postResContent = await postRes.Content.ReadFromJsonAsync<ProductResponseDto>();

        postRes.StatusCode.Should().Be(HttpStatusCode.Created);
        postResContent.Should().BeEquivalentTo(postReqExpected, opt => opt.Excluding(x => x.Id));
        postReqExpected.Id = postResContent.Id;

        var putReq = new ProductItemModel
        {
            Name = "updated",
            Description = "updated",
            PriceRegular = 6,
            PriceDiscount = 6,
            StockAmount = 6,
            Category = "updated"
        };
        putReq.Id = postReqExpected.Id;

        // Act
        var res = await HttpClient.PutAsJsonAsync($"{ProductConstants.ProductEndpoint}/{putReq.Id}", putReq);
        var resContent = await res.Content.ReadFromJsonAsync<ProductResponseDto>();

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        resContent.Should().BeEquivalentTo(putReq);
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