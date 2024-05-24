﻿using AutoMapper;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;
namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests;

public class CreateRestaurantCommandHandlerTests
{
    [Fact()]
    public async Task Handle_ForValideCommand_ReturnCreatedRestaurantId()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();
        var mapperMock = new Mock<IMapper>();

        var command = new CreateRestaurantCommand();
        var restaurant = new Restaurant();
        mapperMock.Setup(m => m.Map<Restaurant>(command)).Returns(restaurant);
        var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantRepositoryMock.Setup(repo => repo.Create(It.IsAny<Restaurant>())).ReturnsAsync(1);
        var userContextMock = new Mock<IUserContext>();
        var currentUser = new CurrentUser("owner-id", "test@gmail.com", [], null, null);
        userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);
        var commandHandler = new CreateRestaurantCommandHandler(loggerMock.Object, 
            mapperMock.Object, 
            restaurantRepositoryMock.Object,
            userContextMock.Object);

        // Act

        var result = await commandHandler.Handle(command, CancellationToken.None);

        // Assert

        result.Should().Be(1);
        restaurant.OwnerId.Should().Be("owner-id");
        restaurantRepositoryMock.Verify(r => r.Create(restaurant), Times.Once);
    }
}