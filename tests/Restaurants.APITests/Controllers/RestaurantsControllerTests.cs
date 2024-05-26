using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Authorization.Policy;
using Restaurants.APITests;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Restaurants.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Restaurants.Domain.Entities;
using System.Net.Http.Json;
using Restaurants.Application.Restaurants.Dtos;
namespace Restaurants.API.Controllers.Tests;

public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{

    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IRestaurantsRepository> _restaurantRepositoryMock = new();
    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    {

        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantsRepository), _ => _restaurantRepositoryMock.Object));
            });
        });

    }
    [Fact()]
    public async Task GetAll_ForValidRequst_Returns200Ok()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act

        var result = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");

        // Assert

        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

    }

    [Fact()]
    public async Task GetAll_ForInvalidRequst_Returns400BadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act

        var result = await client.GetAsync("/api/restaurants");

        // Assert

        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

    }

    [Fact()]
    public async Task GetById_ForNonExistingId_ShouldReturn404NotFound()
    {
        // Arrange
        var id = 1123;
        _restaurantRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync((Restaurant?)null);
        var client = _factory.CreateClient();


        // Act
        var result = await client.GetAsync($"/api/restaurants/{id}");


        // Assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);


    }

    [Fact()]
    public async Task GetById_ForExistingId_ShouldReturn200Ok()
    {
        // Arrange
        var id = 77;
        var restaurant = new Restaurant { Id = 77, Name = "Test", Description = "Description" };
        _restaurantRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(restaurant);
        var client = _factory.CreateClient();


        // Act
        var result = await client.GetAsync($"/api/restaurants/{id}");
        var restaurantDto = await result.Content.ReadFromJsonAsync<RestaurantDto>();

        // Assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        restaurantDto.Should().NotBeNull();
        restaurantDto.Name.Should().Be("Test");
        restaurantDto.Description.Should().Be("Description");


    }
}