using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    private readonly List<string> validCategories = ["Italian", "Japanese", "French", "Indian"];
    public CreateRestaurantCommandValidator()
    {
        RuleFor(form => form.Name)
            .Length(3, 100);
        RuleFor(form => form.Category)
            .Must(category => validCategories.Contains(category)).WithMessage("Invalid category");
        //.Custom((value, context) =>
        //{
        //    var isValidCategory = validCategories.Contains(value);
        //    if(!isValidCategory)
        //    {
        //        context.AddFailure("Category", "Invalid category");
        //    }
        //});

        RuleFor(form => form.ContactEmail)
            .EmailAddress().WithMessage("Please provide valide email address.");
        RuleFor(form => form.PostalCode)
            .Matches(@"^\d{2}-\d{3}$").WithErrorCode("Please provide a valid code (XX-XXX).");
    }
}
