using System.Net;
using System.Net.Http.Json;
using Bogus;
using EcommerceApp.Admin.Test.Abstractions;
using EcommerceApp.Extensions;
using EcommerceApp.Features.Order.DTOs;
using EcommerceApp.Features.Order.Models;
using FluentAssertions;

namespace EcommerceApp.Admin.Test.Features.Order.Endpoints;

public sealed class OrderEndpointIntegrationTest : BaseIntegrationTest
{
    private readonly Faker<OrderModel> _orderGenerator =
        new Faker<OrderModel>()
            .RuleFor(x => x.OrderAddress, f => f.Address.FullAddress())
            .RuleFor(x => x.OrderDate, f => f.Date.RecentOffset().ToUniversalTime().TruncateToPostgresPrecision())
            .RuleFor(x => x.OrderStatus, f => f.Lorem.Word())
            .UseSeed(1006);

    public OrderEndpointIntegrationTest(IntegrationTestWebAppFactoryFixture factoryFixture,
        ITestOutputHelper testOutputHelper) : base(factoryFixture, testOutputHelper)
    {
    }

    [Fact]
    public async Task Create()
    {
        // Arrange
        var generatedModel = _orderGenerator.Generate();
        var createDto = new OrderCreateRequestDto
        {
            OrderAddress = generatedModel.OrderAddress,
            OrderDate = generatedModel.OrderDate,
            OrderStatus = generatedModel.OrderStatus
        };
        var expected = new OrderResponseDto
        {
            Id = 0,
            OrderAddress = generatedModel.OrderAddress,
            OrderDate = generatedModel.OrderDate,
            OrderStatus = generatedModel.OrderStatus
        };

        // Act
        var res = await HttpClient.PostAsJsonAsync(OrderConstants.OrderEndpoint, createDto,
            TestContext.Current.CancellationToken);
        var resContent = await res.Content.ReadFromJsonAsync<OrderResponseDto>(TestContext.Current.CancellationToken);

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.Created);
        resContent.Should().BeEquivalentTo(expected, opt => { return opt.Excluding(x => x.Id); });
    }

    [Fact]
    public async Task GetById()
    {
        // Arrange
        var createdItem = await CreateOrder();

        // Act
        var res = await HttpClient.GetAsync($"{OrderConstants.OrderEndpoint}/{createdItem.Id}",
            TestContext.Current.CancellationToken);

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var resContent = await res.Content.ReadFromJsonAsync<OrderResponseDto>(TestContext.Current.CancellationToken);
        resContent.Should().BeEquivalentTo(createdItem);
    }

    [Fact]
    public async Task UpdateById()
    {
        // Arrange
        var createdItem = await CreateOrder();
        var putReq = new OrderModel
        {
            Id = createdItem.Id,
            OrderAddress = "updated",
            OrderDate = DateTimeOffset.UtcNow,
            OrderStatus = "updated"
        };

        // Act
        var res = await HttpClient.PutAsJsonAsync($"{OrderConstants.OrderEndpoint}/{putReq.Id}", putReq,
            TestContext.Current.CancellationToken);

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var resContent = await res.Content.ReadFromJsonAsync<OrderResponseDto>(TestContext.Current.CancellationToken);
        resContent.Should().BeEquivalentTo(putReq);
    }

    [Fact]
    public async Task DeleteById()
    {
        // Arrange
        var createdItem = await CreateOrder();

        // Act
        var res = await HttpClient.DeleteAsync($"{OrderConstants.OrderEndpoint}/{createdItem.Id}",
            TestContext.Current.CancellationToken);

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    private async Task<OrderResponseDto> CreateOrder()
    {
        // Arrange
        var generatedModel = _orderGenerator.Generate();
        var createDto = new OrderCreateRequestDto
        {
            OrderAddress = generatedModel.OrderAddress,
            OrderDate = generatedModel.OrderDate,
            OrderStatus = generatedModel.OrderStatus
        };
        var expected = new OrderResponseDto
        {
            Id = 0,
            OrderAddress = generatedModel.OrderAddress,
            OrderDate = generatedModel.OrderDate,
            OrderStatus = generatedModel.OrderStatus
        };

        // Act
        var postRes = await HttpClient.PostAsJsonAsync(OrderConstants.OrderEndpoint, createDto);

        // Assert
        postRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var postResContent = await postRes.Content.ReadFromJsonAsync<OrderResponseDto>();
        postResContent.Should().BeEquivalentTo(expected, opt => opt.Excluding(x => x.Id));

        return postResContent;
    }
}