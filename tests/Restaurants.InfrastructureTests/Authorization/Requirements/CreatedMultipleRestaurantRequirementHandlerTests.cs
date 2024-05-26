using Xunit;
using Restaurants.Infrastructure.Authorization.Requirements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using FluentAssertions;

namespace Restaurants.Infrastructure.Authorization.Requirements.Tests
{
    public class CreatedMultipleRestaurantRequirementHandlerTests
    {
        [Fact()]
        public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldSucceed()
        {
            // Arrange

            var currentUser = new CurrentUser("1", "test@gmail.com", [], null, null);
            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);

            var restaurants = new List<Restaurant>()
            {
                new()
                {
                    OwnerId = currentUser.Id,
                },
                new()
                {
                    OwnerId = currentUser.Id,
                },
                new()
                {
                    OwnerId = "2"
                },
            };

            var restaurantRepositiryMock = new Mock<IRestaurantsRepository>();
            restaurantRepositiryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurants);

            var requirement = new CreatedMultipleRestaurantRequirement(2);
            var handler = new CreatedMultipleRestaurantRequirementHandler(restaurantRepositiryMock.Object, userContextMock.Object);
            var context = new AuthorizationHandlerContext([requirement], null, null);

            // Act

            await handler.HandleAsync(context);

            // Assert

            context.HasSucceeded.Should().BeTrue();
        }

        [Fact()]
        public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldFail()
        {
            // Arrange

            var currentUser = new CurrentUser("1", "test@gmail.com", [], null, null);
            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);

            var restaurants = new List<Restaurant>()
            {
                new()
                {
                    OwnerId = currentUser.Id,
                },
                new()
                {
                    OwnerId = "2"
                },
            };

            var restaurantRepositiryMock = new Mock<IRestaurantsRepository>();
            restaurantRepositiryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurants);

            var requirement = new CreatedMultipleRestaurantRequirement(2);
            var handler = new CreatedMultipleRestaurantRequirementHandler(restaurantRepositiryMock.Object, userContextMock.Object);
            var context = new AuthorizationHandlerContext([requirement], null, null);

            // Act

            await handler.HandleAsync(context);

            // Assert

            context.HasSucceeded.Should().BeFalse();
        }
    }
}