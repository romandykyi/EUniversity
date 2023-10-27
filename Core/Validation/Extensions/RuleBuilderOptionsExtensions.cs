using EUniversity.Core.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace EUniversity.Core.Validation.Extensions;

public static class RuleBuilderOptionsExtensions
{
    /// <summary>
    /// Adds a custom validation rule to check 
    /// if the provided user ID corresponds to a valid user in the specified role.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder to which this rule should be added.</param>
    /// <param name="userManager">The user manager to use for user validation.</param>
    /// <param name="roleName">The name of the role to check against.</param>
    /// <returns>
    /// An <see cref="IRuleBuilderOptionsConditions{T,string}"/> object that can be 
    /// used to further configure the validation rule.
    /// </returns>
    /// <remarks>
    /// This method checks if the user with the provided user ID exists and belongs to the specified role.
    /// If the user does not exist or does not have the required role, a validation failure is added.
    /// </remarks>
    public static IRuleBuilderOptionsConditions<T, string> IsIdOfValidUserInRole<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        UserManager<ApplicationUser> userManager,
        string roleName)
    {
        return ruleBuilder.CustomAsync(async (userId, context, cancellationToken) =>
        {
            var user = await userManager.FindByIdAsync(userId);

            // Check if user exists
            if (user == null)
            {
                ValidationFailure failure = new(context.PropertyPath, "User does not exist", userId)
                {
                    ErrorCode = ValidationErrorCodes.InvalidForeignKey
                };
                context.AddFailure(failure);
                return;
            }

            // Check if user has a role
            if (!await userManager.IsInRoleAsync(user, roleName))
            {
                ValidationFailure failure = new(context.PropertyPath, "User does not have the required role", userId)
                {
                    ErrorCode = ValidationErrorCodes.InvalidForeignKey
                };
                context.AddFailure(failure);
            }
        });
    }
}
