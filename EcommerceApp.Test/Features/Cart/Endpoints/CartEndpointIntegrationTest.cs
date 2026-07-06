using System.Net;
using System.Net.Http.Json;
using Bogus;
using EcommerceApp.Features.Cart.DTOs;
using EcommerceApp.Features.Cart.Models;
using EcommerceApp.Test.Abstractions;
using FluentAssertions;

[assembly: CaptureConsole]
namespace EcommerceApp.Test.Features.Cart.Endpoints;

public class CartEndpointIntegrationTest : BaseIntegrationTest
{
    private readonly Faker<CartItemModel> _cartItemGenerator =
        new Faker<CartItemModel>()
            .RuleFor(x => x.ProductsId, f => f.Random.Number())
            .RuleFor(x => x.ItemAmount, f => f.Random.Number())
            .UseSeed(1006);

    public CartEndpointIntegrationTest(IntegrationTestWebAppFactoryFixture factoryFixture, ITestOutputHelper testOutputHelper) : base(factoryFixture, testOutputHelper)
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
            ProductsId = postReq.ProductsId,
            ItemAmount = postReq.ItemAmount,
            CustomersId = userId,
        };
        var expected = new CartItemResponseDto
        {
            Id = 0,
            ProductsId = postReq.ProductsId,
            ItemAmount = postReq.ItemAmount,
            CustomersId = userId,
        };

        // Act
        var res = await HttpClient.PostAsJsonAsync(CartConstants.Endpoint, createDto, cancellationToken: TestContext.Current.CancellationToken);
        var resContent = await res.Content.ReadFromJsonAsync<CartItemResponseDto>();

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.Created);
        resContent.Should().BeEquivalentTo(expected, opt => opt.Excluding(x => x.Id));
    }

    [Fact]
    public async Task GetById()
    {
        // Arrange
        var userId = 3;
        var postReq = _cartItemGenerator.Generate();
        var expected = new CartItemResponseDto
        {
            Id = 0,
            ProductsId = postReq.ProductsId,
            ItemAmount = postReq.ItemAmount,
            CustomersId = userId,
        };

        var postRes = await HttpClient.PostAsJsonAsync(CartConstants.Endpoint, postReq);
        var postResContent = await postRes.Content.ReadFromJsonAsync<CartItemResponseDto>();

        postRes.StatusCode.Should().Be(HttpStatusCode.Created);
        postResContent.Should().BeEquivalentTo(expected, opt => opt.Excluding(x => x.Id));
        expected.Id = postResContent.Id;


        // Act
        var res = await HttpClient.GetAsync($"{CartConstants.Endpoint}/{expected.Id}");
        var resContent = await res.Content.ReadFromJsonAsync<CartItemResponseDto>();

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        resContent.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public async Task UpdateById()
    {
        // Arrange
        var userId = 3;
        var postReq = _cartItemGenerator.Generate();
        var postReqExpected = new CartItemResponseDto
        {
            Id = 0,
            ProductsId = postReq.ProductsId,
            ItemAmount = postReq.ItemAmount,
            CustomersId = userId,
        };

        var postRes = await HttpClient.PostAsJsonAsync(CartConstants.Endpoint, postReq, cancellationToken: TestContext.Current.CancellationToken);
        var postResContent = await postRes.Content.ReadFromJsonAsync<CartItemResponseDto>(cancellationToken: TestContext.Current.CancellationToken);

        postRes.StatusCode.Should().Be(HttpStatusCode.Created);
        postResContent.Should().BeEquivalentTo(postReqExpected, opt => opt.Excluding(x => x.Id));
        postReqExpected.Id = postResContent.Id;

        var putReq = new CartItemUpdateRequestDto()
        {
            ProductsId = 6,
            ItemAmount = 6,
        };
        putReq.Id = postReqExpected.Id;

        // Act
        var res = await HttpClient.PutAsJsonAsync($"{CartConstants.Endpoint}/{putReq.Id}", putReq, cancellationToken: TestContext.Current.CancellationToken);
        var resContent = await res.Content.ReadFromJsonAsync<CartItemResponseDto>(cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        resContent.Should().BeEquivalentTo(putReq);
    }

    [Fact]
    public async Task DeleteById()
    {
        // Arrange
        var userId = 3;
        var postReq = _cartItemGenerator.Generate();
        var postReqExpected = new CartItemResponseDto
        {
            Id = 0,
            ProductsId = postReq.ProductsId,
            ItemAmount = postReq.ItemAmount,
            CustomersId = userId
        };

        var postRes = await HttpClient.PostAsJsonAsync(CartConstants.Endpoint, postReq);
        var postResContent = await postRes.Content.ReadFromJsonAsync<CartItemResponseDto>();

        postRes.StatusCode.Should().Be(HttpStatusCode.Created);
        postResContent.Should().BeEquivalentTo(postReqExpected, opt => opt.Excluding(x => x.Id));
        postReqExpected.Id = postResContent.Id;
        
        // Act
        var res = await HttpClient.DeleteAsync($"{CartConstants.Endpoint}/{postReqExpected.Id}");

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
}