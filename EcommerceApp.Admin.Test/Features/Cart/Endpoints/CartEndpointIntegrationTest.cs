using System.Net;
using System.Net.Http.Json;
using Bogus;
using EcommerceApp.Admin.Test.Abstractions;
using EcommerceApp.Features.Cart.DTOs;
using EcommerceApp.Features.Cart.Models;
using FluentAssertions;

[assembly: CaptureConsole]

namespace EcommerceApp.Admin.Test.Features.Cart.Endpoints;

public sealed class CartEndpointIntegrationTest : BaseIntegrationTest
{
    private readonly Faker<CartItemModel> _cartItemGenerator =
        new Faker<CartItemModel>()
            .RuleFor(x => x.ProductId, f => f.Random.Number())
            .RuleFor(x => x.ItemAmount, f => f.Random.Number())
            .UseSeed(1006);

    public CartEndpointIntegrationTest(IntegrationTestWebAppFactoryFixture factoryFixture,
        ITestOutputHelper testOutputHelper) : base(factoryFixture, testOutputHelper)
    {
    }

    [Fact]
    public async Task Create()
    {
        // Arrange
        var userId = 3;
        var postReq = _cartItemGenerator.Generate();
        var createDto = new CartItemCreateRequestDto
        {
            ProductId = postReq.ProductId,
            ItemAmount = postReq.ItemAmount,
            CustomerId = userId
        };
        var expected = new CartItemResponseDto
        {
            Id = 0,
            ProductId = postReq.ProductId,
            ItemAmount = postReq.ItemAmount,
            CustomerId = userId
        };

        // Act
        var res = await HttpClient.PostAsJsonAsync(CartConstants.Endpoint, createDto);

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.Created);
        var resContent = await res.Content.ReadFromJsonAsync<CartItemResponseDto>();
        resContent.Should().BeEquivalentTo(expected, opt => opt.Excluding(x => x.Id));
    }

    [Fact]
    public async Task GetById()
    {
        // Arrange
        var userId = 3;
        var createdItem = await CreateCartItem(userId);

        // Act
        var res = await HttpClient.GetAsync($"{CartConstants.Endpoint}/{createdItem.Id}");

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var resContent = await res.Content.ReadFromJsonAsync<CartItemResponseDto>();
        resContent.Should().BeEquivalentTo(createdItem);
    }

    [Fact]
    public async Task UpdateById()
    {
        // Arrange
        var userId = 3;
        var createdItem = await CreateCartItem(userId);

        // Act
        var putReq = new CartItemUpdateRequestDto
        {
            Id = createdItem.Id,
            ProductId = 6,
            ItemAmount = 6,
            CustomerId = userId
        };
        var res = await HttpClient.PutAsJsonAsync($"{CartConstants.Endpoint}/{putReq.Id}", putReq);

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var resContent = await res.Content.ReadFromJsonAsync<CartItemResponseDto>();
        resContent.Should().BeEquivalentTo(putReq);
    }

    [Fact]
    public async Task DeleteById()
    {
        // Arrange
        var userId = 3;
        var createdItem = await CreateCartItem(userId);

        // Act
        var res = await HttpClient.DeleteAsync($"{CartConstants.Endpoint}/{createdItem.Id}");

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    private async Task<CartItemResponseDto> CreateCartItem(int userId)
    {
        var postReq = _cartItemGenerator.Generate();
        var postReqExpected = new CartItemResponseDto
        {
            Id = 0,
            ProductId = postReq.ProductId,
            ItemAmount = postReq.ItemAmount,
            CustomerId = userId
        };

        var postRes = await HttpClient.PostAsJsonAsync(CartConstants.Endpoint, postReq);
        postRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var postResContent = await postRes.Content.ReadFromJsonAsync<CartItemResponseDto>();
        postResContent.Should().BeEquivalentTo(postReqExpected, opt => opt.Excluding(x => x.Id));
        postReqExpected.Id = postResContent.Id;

        return postResContent;
    }
}