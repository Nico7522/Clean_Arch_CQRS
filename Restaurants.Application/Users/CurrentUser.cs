namespace Restaurants.Application.Users;

public record CurrentUser(string Id, string Email, IEnumerable<string> Roles, string? nationality, DateOnly? dateOfBirth)
{
    public bool IsInRole(string role) => Roles.Contains(role);
}
