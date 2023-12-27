using EUniversity.Auth;
using EUniversity.Controllers;
using EUniversity.Core.Policy;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace EUniversity.Tests.Auth;

public class AccessOnlyOwnDataAuthorizationHandlerTests
{
    private readonly string TestUserId = Guid.NewGuid().ToString();
    private readonly string TestRouteStudentId = Guid.NewGuid().ToString();

    private ClaimsPrincipal GetUser(string id, params string[] roles)
    {
        List<Claim> claims = new()
        {
            new(JwtClaimTypes.Subject, id)
        };
        foreach (string role in roles)
        {
            claims.Add(new(JwtClaimTypes.Role, role));
        }
        ClaimsIdentity identity = new(claims, "Test");
        return new ClaimsPrincipal(identity);
    }

    private AuthorizationHandlerContext GetHandlerContext(ClaimsPrincipal user, params string[] roles)
    {
        RouteValueDictionary routeValues = new()
        {
            { UsersController.StudentIdRouteKey, TestRouteStudentId }
        };
        HttpContext httpContext = Substitute.For<HttpContext>();
        var routeValuesFeature = Substitute.For<IRouteValuesFeature>();
        routeValuesFeature.RouteValues.Returns(routeValues);

        httpContext.Features.Get<IRouteValuesFeature>().Returns(routeValuesFeature);

        IAuthorizationRequirement[] requirements =
        {
            new AccessOnlyOwnDataAuthorizationRequirement(roles)
        };

        return new(requirements, user, httpContext);
    }

    [Test]
    [TestCase(Roles.Administrator)]
    [TestCase(Roles.Teacher)]
    public async Task SkipRoleAccessesNotOwnData_Succeeds(string role)
    {
        // Arrange
        ClaimsPrincipal user = GetUser(TestUserId, role);
        AuthorizationHandlerContext context = GetHandlerContext(user, Roles.Administrator, Roles.Teacher);
        AccessOnlyOwnDataAuthorizationHandler handler = new();

        // Act
        await handler.HandleAsync(context);

        // Assert
        Assert.That(context.HasSucceeded);
    }

    [Test]
    public async Task UserAccessesOwnData_Succeeds()
    {
        // Arrange
        ClaimsPrincipal user = GetUser(TestRouteStudentId);
        AuthorizationHandlerContext context = GetHandlerContext(user);
        AccessOnlyOwnDataAuthorizationHandler handler = new();

        // Act
        await handler.HandleAsync(context);

        // Assert
        Assert.That(context.HasSucceeded);
    }

    [Test]
    public async Task UserAccessesDataOfAnotherUser_Fails()
    {
        // Arrange
        ClaimsPrincipal user = GetUser(TestUserId);
        AuthorizationHandlerContext context = GetHandlerContext(user);
        AccessOnlyOwnDataAuthorizationHandler handler = new();

        // Act
        await handler.HandleAsync(context);

        // Assert
        Assert.That(context.HasFailed);
    }
}
