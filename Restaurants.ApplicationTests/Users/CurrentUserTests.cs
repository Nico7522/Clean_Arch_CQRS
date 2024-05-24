﻿using FluentAssertions;
using Restaurants.Domain.Constants;
using Xunit;

namespace Restaurants.Application.Users.Tests;

public class CurrentUserTests
{
    [Theory()]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.User)]

    public void IsInRole_WithMatchingRole_ShouldReturnTrue(string roleName)
    {
        // Arrange

        var currentUser = new CurrentUser("1", "test@gmail.com", [UserRoles.Admin, UserRoles.User], null, null);

        // Act

        var isInRole = currentUser.IsInRole(roleName);

        // Assert

        isInRole.Should().BeTrue();
    }

    [Fact()]
    public void IsInRole_WithMatchingRole_ShouldReturnFalse()
    {
        // Arrange

        var currentUser = new CurrentUser("1", "test@gmail.com", [UserRoles.Admin, UserRoles.User], null, null);

        // Act

        var isInRole = currentUser.IsInRole(UserRoles.Owner);

        // Assert

        isInRole.Should().BeFalse();
    }

    [Fact()]
    public void IsInRole_WithMatchingRoleCase_ShouldReturnFalse()
    {
        // Arrange

        var currentUser = new CurrentUser("1", "test@gmail.com", [UserRoles.Admin, UserRoles.User], null, null);

        // Act

        var isInRole = currentUser.IsInRole(UserRoles.Admin.ToLower());

        // Assert

        isInRole.Should().BeFalse();
    }
}