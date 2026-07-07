using System.Net;
using System.Net.Http.Json;
using Bogus;
using EcommerceApp.Features.Cart.DTOs;
using EcommerceApp.Features.Cart.Models;
using EcommerceApp.Test.Abstractions;
using FluentAssertions;

namespace EcommerceApp.Test.Features.Cart.Endpoints;

public sealed class GetAll : BaseIntegrationTest
{
    private readonly Faker<CartItemModel> _cartGenerator =
        new Faker<CartItemModel>()
            .RuleFor(x => x.ItemAmount, f => f.Random.Number())
            .RuleFor(x => x.ProductId, f => f.Random.Number())
            .UseSeed(1000);

    public GetAll(IntegrationTestWebAppFactoryFixture factoryFixture, ITestOutputHelper testOutputHelper) : base(
        factoryFixture, testOutputHelper)
    {
    }

    [Fact]
    public async Task GetAll_ReturnsAll()
    {
        var userId = 3;
        // Arrange
        var postReqList = new List<CartItemModel>
        {
            _cartGenerator.Generate(),
            _cartGenerator.Generate(),
            _cartGenerator.Generate(),
            _cartGenerator.Generate()
        };
        var createdItemList = new List<CartItemResponseDto>();
        foreach (var item in postReqList)
        {
            var dto = new CartItemCreateRequestDto
            {
                ProductId = item.ProductId,
                ItemAmount = item.ItemAmount,
                CustomerId = userId
            };
            var postRes = await HttpClient.PostAsJsonAsync(CartConstants.Endpoint, dto);
            var postResContent = await postRes.Content.ReadFromJsonAsync<CartItemResponseDto>();
            postRes.StatusCode.Should().Be(HttpStatusCode.Created);
            postResContent.Should().BeEquivalentTo(dto);
            createdItemList.Add(postResContent);
        }

        // Act
        var res = await HttpClient.GetAsync(CartConstants.Endpoint);
        var resContent = await res.Content.ReadFromJsonAsync<List<CartItemResponseDto>>();

        // Assert
        var expected = createdItemList;
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        resContent.Should().BeEquivalentTo(expected);
    }
}