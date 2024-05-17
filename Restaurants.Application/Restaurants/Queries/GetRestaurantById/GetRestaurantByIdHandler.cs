﻿

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

public class GetRestaurantByIdHandler(IRestaurantsRepository restaurantsRepository, ILogger<GetRestaurantByIdHandler> logger, IMapper mapper) : IRequestHandler<GetRestaurantByIdQuery, RestaurantDto?>
{
    public async Task<RestaurantDto?> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Get");
        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id);
        var restaurantsDtos = mapper.Map<RestaurantDto?>(restaurant);
        return restaurantsDtos;
    }
}