using System.Net;
using System.Net.Http.Json;
using Bogus;
using EcommerceApp.Admin.Test.Abstractions;
using EcommerceApp.Features.Category.DTOs;
using EcommerceApp.Features.Category.Models;
using FluentAssertions;

namespace EcommerceApp.Admin.Test.Features.Category.Endpoints;

public sealed class CategoryEndpointIntegrationTest : BaseIntegrationTest
{
    private readonly Faker<CategoryModel> _orderGenerator =
        new Faker<CategoryModel>()
            .RuleFor(x => x.Name, f => f.Random.Word())
            .RuleFor(x => x.Description, f => f.Random.Words())
            .UseSeed(1006);

    public CategoryEndpointIntegrationTest(IntegrationTestWebAppFactoryFixture factoryFixture,
        ITestOutputHelper testOutputHelper) : base(factoryFixture, testOutputHelper)
    {
    }

    [Fact]
    public async Task Create()
    {
        // Arrange
        var postReq = _orderGenerator.Generate();
        var createDto = new CategoryCreateRequestDto
        {
            Name = postReq.Name,
            Description = postReq.Description
        };
        var expected = new CategoryResponseDto
        {
            Id = 0,
            Name = postReq.Name,
            Description = postReq.Description,
        };

        // Act
        var res = await HttpClient.PostAsJsonAsync(CategoryConstants.Endpoint, createDto,
            TestContext.Current.CancellationToken);

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.Created);
        var resContent = await res.Content.ReadFromJsonAsync<CategoryResponseDto>();
        resContent.Should().BeEquivalentTo(expected, opt => opt.Excluding(x => x.Id));
    }

    [Fact]
    public async Task GetById()
    {
        // Arrange
        var createdItem = await CreateCategory();

        // Act
        var res = await HttpClient.GetAsync($"{CategoryConstants.Endpoint}/{createdItem.Id}");

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var resContent = await res.Content.ReadFromJsonAsync<CategoryResponseDto>();
        resContent.Should().BeEquivalentTo(createdItem);
    }

    [Fact]
    public async Task UpdateById()
    {
        // Arrange
        var createdItem = await CreateCategory();

        // Act
        var putReq = new CategoryModel
        {
            Name = "updated",
            Description = "updated"
        };
        putReq.Id = createdItem.Id;
        var res = await HttpClient.PutAsJsonAsync($"{CategoryConstants.Endpoint}/{putReq.Id}", putReq,
            TestContext.Current.CancellationToken);

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var resContent = await res.Content.ReadFromJsonAsync<CategoryResponseDto>(TestContext.Current.CancellationToken);
        resContent.Should().BeEquivalentTo(putReq);
    }

    [Fact]
    public async Task DeleteById()
    {
        // Arrange
        var createdItem = await CreateCategory();

        // Act
        var res = await HttpClient.DeleteAsync($"{CategoryConstants.Endpoint}/{createdItem.Id}");

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    private async Task<CategoryResponseDto> CreateCategory()
    {
        // Arrange
        var postReq = _orderGenerator.Generate();
        var postReqExpected = new CategoryResponseDto
        {
            Id = 0,
            Name = postReq.Name,
            Description = postReq.Description,
        };

        // Act
        var postRes = await HttpClient.PostAsJsonAsync(CategoryConstants.Endpoint, postReq);
        
        // Assert
        postRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var postResContent = await postRes.Content.ReadFromJsonAsync<CategoryResponseDto>();
        postResContent.Should().BeEquivalentTo(postReqExpected, opt => opt.Excluding(x => x.Id));
        postReqExpected.Id = postResContent.Id;

        return postResContent;
    }
}