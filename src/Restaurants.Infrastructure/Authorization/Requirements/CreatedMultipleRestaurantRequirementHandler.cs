using Microsoft.AspNetCore.Authorization;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements;

internal class CreatedMultipleRestaurantRequirementHandler(IRestaurantsRepository restaurantsRepository,
    IUserContext userContext) : AuthorizationHandler<CreatedMultipleRestaurantRequirement>
{

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();
        var restaurants = await restaurantsRepository.GetAllAsync();

        var userRestaurantCreated = restaurants.Count(r => r.OwnerId == currentUser!.Id);

        if(userRestaurantCreated >= requirement.MinimumRestaurantCreated)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

    }
}
