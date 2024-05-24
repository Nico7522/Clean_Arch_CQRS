using Xunit;
using Restaurants.Application.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Restaurants.Domain.Constants;
using FluentAssertions;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Users.Tests
{
    public class UserContextTests
    {
        [Fact()]
        public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
        {
            // Arrange
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var dateOfBirth = new DateOnly(1990, 1, 1);
            var claims = new List<Claim>() {
                new(ClaimTypes.NameIdentifier, "1"),
                new(ClaimTypes.Email, "test@gmail.com"),
                new(ClaimTypes.Role, UserRoles.Admin),
                new(ClaimTypes.Role, UserRoles.User),
                new("Nationality", "German"),
                new("DateOfBirth", dateOfBirth.ToString("yyyy-MM-dd")),
            };


            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "test"));

            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext()
            {
                User = user
            }) ;
            var userContext = new UserContext(httpContextAccessorMock.Object);


            // Act

            var currentUser = userContext.GetCurrentUser();

            // Asset
            currentUser.Should().NotBeNull();
            currentUser.Id.Should().Be("1");
            currentUser.Email.Should().Be("test@gmail.com");
            currentUser.Roles.Should().ContainInOrder(UserRoles.Admin, UserRoles.User);
            currentUser.nationality.Should().Be("German");
            currentUser.dateOfBirth.Should().Be(dateOfBirth);


        }

        [Fact()]
        public void GetCurrentUser_WithUserContextNotPresent_ThrowsInvalidOperationException()
        {

            // Arrange
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);
            var userContext = new UserContext(httpContextAccessorMock.Object);

            // Act

            Action action = () => userContext.GetCurrentUser();

            // Assert

            action.Should().Throw<InvalidOperationException>().WithMessage("User context is not present");

        }
    }
}