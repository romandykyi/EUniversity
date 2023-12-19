using Duende.IdentityServer.Extensions;
using EUniversity.Controllers;
using EUniversity.Core.Policy;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;

namespace EUniversity.Auth;

/// <summary>
/// Authorization handler that determines whether user can view students enrollments.
/// Allows teachers and administrators to view all enrollments and other users to view
/// only their own enrollments.
/// </summary>
public class ViewStudentEnrollmentsAuthorizationHandler :
    AuthorizationHandler<ViewStudentEnrollmentsAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ViewStudentEnrollmentsAuthorizationRequirement requirement)
    {
        if (!context.User.IsAuthenticated())
        {
            context.Fail();
            return Task.CompletedTask;
        }
        // If user is either a teacher or an administrator,
        // he/she has an access to all enrollments
        if (context.User.HasClaim(JwtClaimTypes.Role, Roles.Teacher) ||
            context.User.HasClaim(JwtClaimTypes.Role, Roles.Administrator))
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
