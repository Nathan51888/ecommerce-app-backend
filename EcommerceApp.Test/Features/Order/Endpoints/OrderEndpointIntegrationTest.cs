using System.Net;
using System.Net.Http.Json;
using Bogus;
using EcommerceApp.Features.Order.DTOs;
using EcommerceApp.Features.Order.Models;
using EcommerceApp.Test.Abstractions;
using FluentAssertions;

namespace EcommerceApp.Test.Features.Order.Endpoints;

public class OrderEndpointIntegrationTest : BaseIntegrationTest
{
    private readonly Faker<OrderModel> _orderGenerator =
        new Faker<OrderModel>()
            .RuleFor(x => x.OrderAddress, f => f.Address.FullAddress())
            .RuleFor(x => x.OrderStatus, f => f.Lorem.Word())
            .UseSeed(1006);

    public OrderEndpointIntegrationTest(IntegrationTestWebAppFactoryFixture factoryFixture, ITestOutputHelper testOutputHelper) : base(factoryFixture, testOutputHelper)
    {
    }
    
    [Fact]
    public async Task Create()
    {
        // Arrange
        var postReq = _orderGenerator.Generate();
        var createDto = new OrderCreateRequestDto
        {
            OrderAddress = postReq.OrderAddress,
            OrderStatus = postReq.OrderStatus
        };
        var expected = new OrderResponseDto
        {
            Id = 0,
            OrderAddress = postReq.OrderAddress,
            OrderStatus = postReq.OrderStatus,
        };

        // Act
        var res = await HttpClient.PostAsJsonAsync(OrderConstants.OrderEndpoint, createDto, cancellationToken: TestContext.Current.CancellationToken);
        var resContent = await res.Content.ReadFromJsonAsync<OrderResponseDto>();

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.Created);
        resContent.Should().BeEquivalentTo(expected, opt => opt.Excluding(x => x.Id));
    }

    [Fact]
    public async Task GetById()
    {
        // Arrange
        var postReq = _orderGenerator.Generate();
        var expected = new OrderResponseDto
        {
            Id = 0,
            OrderAddress = postReq.OrderAddress,
            OrderStatus = postReq.OrderStatus,

        };

        var postRes = await HttpClient.PostAsJsonAsync(OrderConstants.OrderEndpoint, postReq);
        var postResContent = await postRes.Content.ReadFromJsonAsync<OrderResponseDto>();

        postRes.StatusCode.Should().Be(HttpStatusCode.Created);
        postResContent.Should().BeEquivalentTo(expected, opt => opt.Excluding(x => x.Id));
        expected.Id = postResContent.Id;


        // Act
        var res = await HttpClient.GetAsync($"{OrderConstants.OrderEndpoint}/{expected.Id}");
        var resContent = await res.Content.ReadFromJsonAsync<OrderResponseDto>();

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        resContent.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task UpdateById()
    {
        // Arrange
        var postReq = _orderGenerator.Generate();
        var postReqExpected = new OrderResponseDto
        {
            Id = 0,
            OrderAddress = postReq.OrderAddress,
            OrderStatus = postReq.OrderStatus,

        };

        var postRes = await HttpClient.PostAsJsonAsync(OrderConstants.OrderEndpoint, postReq, cancellationToken: TestContext.Current.CancellationToken);
        var postResContent = await postRes.Content.ReadFromJsonAsync<OrderResponseDto>(cancellationToken: TestContext.Current.CancellationToken);

        postRes.StatusCode.Should().Be(HttpStatusCode.Created);
        postResContent.Should().BeEquivalentTo(postReqExpected, opt => opt.Excluding(x => x.Id));
        postReqExpected.Id = postResContent.Id;

        var putReq = new OrderModel()
        {
            OrderAddress = "updated",
            OrderStatus = "updated"
        };
        putReq.Id = postReqExpected.Id;

        // Act
        var res = await HttpClient.PutAsJsonAsync($"{OrderConstants.OrderEndpoint}/{putReq.Id}", putReq, cancellationToken: TestContext.Current.CancellationToken);
        var resContent = await res.Content.ReadFromJsonAsync<OrderResponseDto>(cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        resContent.Should().BeEquivalentTo(putReq);
    }

    [Fact]
    public async Task DeleteById()
    {
        // Arrange
        var postReq = _orderGenerator.Generate();
        var postReqExpected = new OrderResponseDto
        {
            Id = 0,
            OrderAddress = postReq.OrderAddress,
            OrderDate = default,
            OrderStatus = postReq.OrderStatus,
        };

        var postRes = await HttpClient.PostAsJsonAsync(OrderConstants.OrderEndpoint, postReq);
        var postResContent = await postRes.Content.ReadFromJsonAsync<OrderResponseDto>();

        postRes.StatusCode.Should().Be(HttpStatusCode.Created);
        postResContent.Should().BeEquivalentTo(postReqExpected, opt => opt.Excluding(x => x.Id));
        postReqExpected.Id = postResContent.Id;
        
        // Act
        var res = await HttpClient.DeleteAsync($"{OrderConstants.OrderEndpoint}/{postReqExpected.Id}");

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
}