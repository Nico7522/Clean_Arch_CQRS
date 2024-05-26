using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Infrastructure.Authorization.Services;

public class RestaurantAuthorizationService(ILogger<RestaurantAuthorizationService> logger,
    IUserContext userContext) : IRestaurantAuthorizationService
{
    public bool Authorize(Restaurant restaurant, ResourceOperation operation)
    {
        var user = userContext.GetCurrentUser();
        logger.LogInformation("Authorazing user {UserEmail}, to {Operation} for restaurant {RestaurantName}", user.Email, operation, restaurant.Name);

        if (operation == ResourceOperation.Read || operation == ResourceOperation.Create)
        {
            logger.LogInformation("Create or read");
            return true;
        }

        if (operation == ResourceOperation.Delete && user.IsInRole(UserRoles.Admin))
        {
            logger.LogInformation("Admin / Delete");
        }

        if (operation == ResourceOperation.Delete || operation == ResourceOperation.Update && user.Id == restaurant.OwnerId)
        {
            logger.LogInformation("Restaurant owner - OK");
            return true;
        }

        return false;

    }
}
