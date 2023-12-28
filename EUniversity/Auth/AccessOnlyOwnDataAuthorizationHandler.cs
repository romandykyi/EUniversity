using Duende.IdentityServer.Extensions;
using EUniversity.Controllers;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;

namespace EUniversity.Auth;

/// <summary>
/// Authorization handler that allows students access only their own data.
/// <see cref="AccessOnlyOwnDataAuthorizationRequirement" /> can contain roles
/// that are allowed to access any data.
/// </summary>
/// <remarks>
/// This handler checks route parameter 'studentId'.
/// </remarks>
public class AccessOnlyOwnDataAuthorizationHandler :
    AuthorizationHandler<AccessOnlyOwnDataAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessOnlyOwnDataAuthorizationRequirement requirement)
    {
        if (!context.User.IsAuthenticated())
        {
            context.Fail();
            return Task.CompletedTask;
        }
        // Check if we can skip the check
        if (requirement.SkipRoles.Any(r => context.User.HasClaim(JwtClaimTypes.Role, r)))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        // Get an ID from route values
        if (context.Resource is not HttpContext httpContext)
        {
            throw new InvalidOperationException("Cannot access HTTP context");
        }
        object? routeId = httpContext.GetRouteValue(UsersController.StudentIdRouteKey) ??
            throw new InvalidOperationException($"Cannot get route value of '{UsersController.StudentIdRouteKey}'");
        // Each user can view his/her enrollments
        if (context.User.GetSubjectId() == routeId.ToString())
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        context.Fail();
        return Task.CompletedTask;
    }
}
