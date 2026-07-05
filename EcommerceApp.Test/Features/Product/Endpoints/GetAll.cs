using System.Net;
using System.Net.Http.Json;
using Bogus;
using EcommerceApp.Features.Products.DTOs;
using EcommerceApp.Features.Products.Models;
using EcommerceApp.Test.Abstractions;
using FluentAssertions;

namespace EcommerceApp.Test.Features.Product.Endpoints;

public class GetAll : BaseIntegrationTest
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

    public GetAll(IntegrationTestWebAppFactoryFixture factoryFixture, ITestOutputHelper testOutputHelper) : base(factoryFixture, testOutputHelper)
    {
    }

    [Fact]
    public async Task GetAll_ReturnsAll()
    {
        // Arrange
        var postReqList = new List<ProductItemModel>
        {
            _productGenerator.Generate(),
            _productGenerator.Generate(),
            _productGenerator.Generate(),
            _productGenerator.Generate()
        };
        var expected = new List<ProductResponseDto>();
        foreach (var item in postReqList)
        {
            var postRes = await HttpClient.PostAsJsonAsync(ProductConstants.ProductEndpoint, item);
            var postResContent = await postRes.Content.ReadFromJsonAsync<ProductResponseDto>();
            postRes.StatusCode.Should().Be(HttpStatusCode.Created);
            expected.Add(postResContent);
        }

        // Act
        var res = await HttpClient.GetAsync(ProductConstants.ProductEndpoint);
        var resContent = await res.Content.ReadFromJsonAsync<List<ProductResponseDto>>();

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        resContent.Should().BeEquivalentTo(expected);
    }
}