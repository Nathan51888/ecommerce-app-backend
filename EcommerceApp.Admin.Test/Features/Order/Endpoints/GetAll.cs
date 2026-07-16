using System.Net;
using System.Net.Http.Json;
using Bogus;
using EcommerceApp.Admin.Test.Abstractions;
using EcommerceApp.Extensions;
using EcommerceApp.Features.Order.DTOs;
using EcommerceApp.Features.Order.Models;
using FluentAssertions;

namespace EcommerceApp.Admin.Test.Features.Order.Endpoints;

public sealed class GetAll : BaseIntegrationTest
{
    private readonly Faker<OrderModel> _orderGenerator =
        new Faker<OrderModel>()
            .RuleFor(x => x.OrderAddress, f => f.Address.FullAddress())
            .RuleFor(x => x.OrderDate, f => f.Date.RecentOffset().ToUniversalTime().TruncateToPostgresPrecision())
            .RuleFor(x => x.OrderStatus, f => f.Lorem.Word())
            .UseSeed(1000);

    public GetAll(IntegrationTestWebAppFactoryFixture factoryFixture, ITestOutputHelper testOutputHelper) : base(
        factoryFixture, testOutputHelper)
    {
    }

    [Fact]
    public async Task GetAll_ReturnsAll()
    {
        // Arrange
        var postReqList = new List<OrderModel>
        {
            _orderGenerator.Generate(),
            _orderGenerator.Generate(),
            _orderGenerator.Generate(),
            _orderGenerator.Generate()
        };
        var expected = new List<OrderResponseDto>();
        foreach (var item in postReqList)
        {
            var postRes = await HttpClient.PostAsJsonAsync(OrderConstants.OrderEndpoint, item);
            var postResContent = await postRes.Content.ReadFromJsonAsync<OrderResponseDto>();
            postRes.StatusCode.Should().Be(HttpStatusCode.Created);
            expected.Add(postResContent);
        }

        // Act
        var res = await HttpClient.GetAsync(OrderConstants.OrderEndpoint);
        var resContent = await res.Content.ReadFromJsonAsync<List<OrderResponseDto>>();

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        resContent.Should().BeEquivalentTo(expected);
    }
}