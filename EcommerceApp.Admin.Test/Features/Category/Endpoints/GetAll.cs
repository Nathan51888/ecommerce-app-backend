using System.Net;
using System.Net.Http.Json;
using Bogus;
using EcommerceApp.Admin.Test.Abstractions;
using EcommerceApp.Features.Category.DTOs;
using EcommerceApp.Features.Category.Models;
using FluentAssertions;

namespace EcommerceApp.Admin.Test.Features.Category.Endpoints;

public sealed class GetAll : BaseIntegrationTest
{
    private readonly Faker<CategoryModel> _categoryGenerator =
        new Faker<CategoryModel>()
            .RuleFor(x => x.Name, f => f.Random.Word())
            .RuleFor(x => x.Description, f => f.Random.Words())
            .UseSeed(1000);

    public GetAll(IntegrationTestWebAppFactoryFixture factoryFixture, ITestOutputHelper testOutputHelper) : base(
        factoryFixture, testOutputHelper)
    {
    }

    [Fact]
    public async Task GetAll_ReturnsAll()
    {
        // Arrange
        var postReqList = new List<CategoryModel>
        {
            _categoryGenerator.Generate(),
            _categoryGenerator.Generate(),
            _categoryGenerator.Generate(),
            _categoryGenerator.Generate()
        };
        var expected = new List<CategoryResponseDto>();
        foreach (var item in postReqList)
        {
            var postRes = await HttpClient.PostAsJsonAsync(CategoryConstants.Endpoint, item);
            var postResContent = await postRes.Content.ReadFromJsonAsync<CategoryResponseDto>();
            postRes.StatusCode.Should().Be(HttpStatusCode.Created);
            expected.Add(postResContent);
        }

        // Act
        var res = await HttpClient.GetAsync(CategoryConstants.Endpoint);
        var resContent = await res.Content.ReadFromJsonAsync<List<CategoryResponseDto>>();

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        resContent.Should().BeEquivalentTo(expected);
    }
}