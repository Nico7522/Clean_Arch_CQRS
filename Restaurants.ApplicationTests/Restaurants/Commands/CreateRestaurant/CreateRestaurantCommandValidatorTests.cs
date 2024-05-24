using FluentValidation.TestHelper;
using Xunit;


namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests;

public class CreateRestaurantCommandValidatorTests
{
    [Fact()]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange

        var command = new CreateRestaurantCommand()
        {
            Name = "Name",
            Category = "Italian",
            ContactEmail = "Test@gmail.com",
            PostalCode = "12-345",
        };

        var validator = new CreateRestaurantCommandValidator();

        // Act

        var result = validator.TestValidate(command);


        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact()]
    public void Validator_ForValidCommand_ShouldHaveValidationErrors()
    {
        // Arrange

        var command = new CreateRestaurantCommand()
        {
            Name = "Na",
            Category = "Itali",
            ContactEmail = "Testgmail.com",
            PostalCode = "1245",
        };

        var validator = new CreateRestaurantCommandValidator();

        // Act

        var result = validator.TestValidate(command);


        // Assert

        result.ShouldHaveValidationErrorFor(c => c.Name);
        result.ShouldHaveValidationErrorFor(c => c.Category);
        result.ShouldHaveValidationErrorFor(c => c.ContactEmail);
        result.ShouldHaveValidationErrorFor(c => c.PostalCode);

    }

    [Theory()]
    [InlineData("Italian")]
    [InlineData("Japanese")]
    [InlineData("French")]
    [InlineData("Indian")]
    public void Validator_ForValidCategory_ShouldNotHaveValidationErrorsforCategoryProperty(string category)
    {
        // Arrange
        var validator = new CreateRestaurantCommandValidator();

        var command = new CreateRestaurantCommand()
        {
            Category = category,
        
        };


        // Act

        var result = validator.TestValidate(command);


        // Assert

        result.ShouldNotHaveValidationErrorFor(c => c.Category);
    }

    [Theory()]
    [InlineData("10222")]
    [InlineData("102-10")]
    [InlineData("10 222")]
    [InlineData("10-2 10")]

    public void Validator_ForInvalidPostalCode_ShouldHaveValidationErrorsForPotalCodeProperty(string postalCode)
    {

        // Arrange
       var validator = new CreateRestaurantCommandValidator();

        var command = new CreateRestaurantCommand()
        {
            PostalCode = postalCode,

        };


        // Act

       var result = validator.TestValidate(command);


        // Assert

        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
    }

}