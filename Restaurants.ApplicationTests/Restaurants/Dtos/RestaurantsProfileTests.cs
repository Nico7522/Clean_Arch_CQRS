using AutoMapper;
using FluentAssertions;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Entities;
using Xunit;


namespace Restaurants.Application.Restaurants.Dtos.Tests;

public class RestaurantsProfileTests
{
    private IMapper _mapper;
    public RestaurantsProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RestaurantsProfile>();
        });

        _mapper = configuration.CreateMapper();
    }
    [Fact()]
    public void CreateMap_ForRestaurantToRestaurantDto_MapCorrectly()
    {
        // Arrange

        var resaurant = new Restaurant()
        {
            Id = 1,
            Name = "Test",
            Category = "Italian",
            HasDelivery = true,
            ContactEmail = "Test@gmail.com",
            ContactNumber = "12345678",
            Address = new Address { City = "test", Street = "street", PostalCode = "12-345" },
        };

        // Act

        var restaurantDto = _mapper.Map<RestaurantDto>(resaurant);

        // Assert

        restaurantDto.Should().NotBeNull();
        restaurantDto.Id.Should().Be(resaurant.Id);
        restaurantDto.Name.Should().Be(resaurant.Name);
        restaurantDto.Description.Should().Be(resaurant.Description);
        restaurantDto.Category.Should().Be(resaurant.Category);
        restaurantDto.HasDelivery.Should().Be(resaurant.HasDelivery);
        restaurantDto.City.Should().Be(resaurant.Address.City);
        restaurantDto.Street.Should().Be(resaurant.Address.Street);
        restaurantDto.PostalCode.Should().Be(resaurant.Address.PostalCode);

    }

    [Fact()]
    public void CreateMap_ForCreateRestaurantCommandToRestaurant_MapCorrectly()
    {
        // Arrange

        var resaurant = new CreateRestaurantCommand()
        {
            Name = "Test",
            Category = "Italian",
            HasDelivery = true,
            ContactEmail = "Test@gmail.com",
            ContactNumber = "12345678",
            City = "test", Street = "street", PostalCode = "12-345"
        };

        // Act

        var restaurantDto = _mapper.Map<Restaurant>(resaurant);

        // Assert

        restaurantDto.Should().NotBeNull();
        restaurantDto.Name.Should().Be(resaurant.Name);
        restaurantDto.Description.Should().Be(resaurant.Description);
        restaurantDto.Category.Should().Be(resaurant.Category);
        restaurantDto.HasDelivery.Should().Be(resaurant.HasDelivery);
        restaurantDto.Address.City.Should().Be(resaurant.City);
        restaurantDto.Address.Street.Should().Be(resaurant.Street);
        restaurantDto.Address.PostalCode.Should().Be(resaurant.PostalCode);

    }

    [Fact()]
    public void CreateMap_ForUpdateRestaurantCommandToRestaurant_MapCorrectly()
    {
        // Arrange

        var resaurant = new UpdateRestaurantCommand()
        {
            Id = 1,
            Name = "Test",
            Description = "desc",
            HasDelivery = true,
        };

        // Act

        var restaurantDto = _mapper.Map<Restaurant>(resaurant);

        // Assert

        restaurantDto.Should().NotBeNull();
        restaurantDto.Id.Should().Be(resaurant.Id);
        restaurantDto.Name.Should().Be(resaurant.Name);
        restaurantDto.Description.Should().Be(resaurant.Description);
        restaurantDto.HasDelivery.Should().Be(resaurant.HasDelivery);

    }
}