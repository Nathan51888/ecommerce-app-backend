using System.Net;
using System.Net.Http.Json;
using Bogus;
using EcommerceApp.Features.Order.DTOs;
using EcommerceApp.Features.Order.Models;
using EcommerceApp.Test.Abstractions;
using FluentAssertions;

[assembly:CaptureConsole]
namespace EcommerceApp.Test.Features.Order.Endpoints;

public class OrderEndpointIntegrationTest : BaseIntegrationTest
{
    private readonly Faker<OrderModel> _orderGenerator =
        new Faker<OrderModel>()
            .RuleFor(x => x.OrderAddress, f => f.Address.FullAddress())
            .RuleFor(x => x.OrderDate, f => f.Date.Soon())
            .RuleFor(x => x.OrderStatus, f => f.Lorem.Word())
            .UseSeed(1000);

    public OrderEndpointIntegrationTest(IntegrationTestWebAppFactory factory, ITestOutputHelper testOutputHelper) : base(factory, testOutputHelper)
    {
    }
    
    [Fact]
    public async Task Create()
    {
        // Arrange
        var postReq = _orderGenerator.Generate();
        var expected = new OrderResponseDto
        {
            Id = 0,
            OrderAddress = postReq.OrderAddress,
            OrderDate = postReq.OrderDate,
            OrderStatus = postReq.OrderStatus,
        };

        // Act
        var res = await HttpClient.PostAsJsonAsync(OrderConstants.OrderEndpoint, postReq);
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
            OrderDate = postReq.OrderDate,
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
            OrderDate = postReq.OrderDate,
            OrderStatus = postReq.OrderStatus,

        };

        var postRes = await HttpClient.PostAsJsonAsync(OrderConstants.OrderEndpoint, postReq);
        var postResContent = await postRes.Content.ReadFromJsonAsync<OrderResponseDto>();

        postRes.StatusCode.Should().Be(HttpStatusCode.Created);
        postResContent.Should().BeEquivalentTo(postReqExpected, opt => opt.Excluding(x => x.Id));
        postReqExpected.Id = postResContent.Id;

        var putReq = new OrderModel()
        {
            OrderAddress = "updated",
            OrderDate = DateTime.Now,
            OrderStatus = "updated"
        };
        putReq.Id = postReqExpected.Id;

        // Act
        var res = await HttpClient.PutAsJsonAsync($"{OrderConstants.OrderEndpoint}/{putReq.Id}", putReq);
        var resContent = await res.Content.ReadFromJsonAsync<OrderResponseDto>();

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        resContent.Should().BeEquivalentTo(putReq);
    }

    [Fact]
    public async Task DeleteById()
    {
        // Arrange
        var postReq = _orderGenerator.Generate();
        var postReqExpected = new OrderResponseDto()
        {
            Id = 0,
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