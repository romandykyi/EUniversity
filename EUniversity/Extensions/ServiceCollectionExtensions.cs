﻿using EUniversity.Auth;
using EUniversity.Core.Models;
using EUniversity.Core.Policy;
using EUniversity.Core.Services;
using EUniversity.Core.Services.Auth;
using EUniversity.Core.Services.University;
using EUniversity.Core.Services.University.Grades;
using EUniversity.Core.Services.Users;
using EUniversity.Core.Validation.Auth;
using EUniversity.Infrastructure.Data;
using EUniversity.Infrastructure.Identity;
using EUniversity.Infrastructure.Services;
using EUniversity.Infrastructure.Services.Auth;
using EUniversity.Infrastructure.Services.Test;
using EUniversity.Infrastructure.Services.University;
using EUniversity.Infrastructure.Services.University.Grades;
using EUniversity.Infrastructure.Services.Users;
using FluentValidation;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Enums;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Net;
using System.Reflection;
using System.Text.Json.Serialization;

namespace EUniversity.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        return services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Title = "E-University",
                Version = "v1"
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }

    public static IServiceCollection AddCustomizedIdentity(this IServiceCollection services)
    {
        services
            .AddIdentity<ApplicationUser, IdentityRole>(
            options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 3;

                options.User.RequireUniqueEmail = true;
                // Alphanumeric characters with dashes, underscores and periods
                options.User.AllowedUserNameCharacters = ApplicationUser.AllowedUserNameCharacters;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services
            .AddIdentityCore<ApplicationUser>(o =>
        {
            o.Stores.MaxLengthForKeys = 128;
        })
            .AddDefaultTokenProviders();

        services.AddIdentityServer()
            .AddApiAuthorization<ApplicationUser, ApplicationDbContext>()
            .AddProfileService<CustomProfileService>();

        services.AddAuthentication()
            .AddJwtBearer();

        services.AddTransient<IClaimsTransformation, CustomClaimsTransform>();

        return services.ConfigureApplicationCookie(options =>
        {
            // Return 401 when user is not authorized
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return Task.CompletedTask;
            };
            // Return 403 when user don't have access permission
            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return Task.CompletedTask;
            };
        });
    }

    public static IServiceCollection AddCustomizedAuthorization(this IServiceCollection services, params string[] authenticationSchemes)
    {
        services.AddTransient<IAuthorizationHandler, AccessOnlyOwnDataAuthorizationHandler>();

        return services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.Default, policy =>
            {
                policy.AddAuthenticationSchemes(authenticationSchemes);
                policy.RequireAuthenticatedUser();
            });
            // Simple role-based policies
            options.AddPolicy(Policies.IsStudent, policy =>
            {
                policy.AddAuthenticationSchemes(authenticationSchemes);
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(JwtClaimTypes.Role, Roles.Student);
            });
            options.AddPolicy(Policies.IsTeacher, policy =>
            {
                policy.AddAuthenticationSchemes(authenticationSchemes);
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(JwtClaimTypes.Role, Roles.Teacher);
            });
            options.AddPolicy(Policies.IsTeacherOrAdministrator, policy =>
            {
                policy.AddAuthenticationSchemes(authenticationSchemes);
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(JwtClaimTypes.Role, Roles.Teacher, Roles.Administrator);
            });
            options.AddPolicy(Policies.HasAdministratorPermission, policy =>
            {
                policy.AddAuthenticationSchemes(authenticationSchemes);
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(JwtClaimTypes.Role, Roles.Administrator);
            });
            // Policies that use authorization requirements
            options.AddPolicy(Policies.CanViewStudentEnrollments, policy =>
            {
                policy.AddAuthenticationSchemes(authenticationSchemes);
                policy.RequireAuthenticatedUser();
                policy.AddRequirements(new AccessOnlyOwnDataAuthorizationRequirement(Roles.Teacher, Roles.Administrator));
            });
            options.DefaultPolicy = options.GetPolicy(Policies.Default)!;
        });
    }

    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<LogInDtoValidator>();
        return services.AddFluentValidationAutoValidation(configuration =>
        {
            // Don't disable built in validation because without it some auth logic
            // doesn't work
            configuration.DisableBuiltInModelValidation = false;

            // Only validate controllers decorated with the `FluentValidationAutoValidation` attribute.
            configuration.ValidationStrategy = ValidationStrategy.Annotations;

            // Enable validation for parameters bound from the `BindingSource.Body` binding source.
            configuration.EnableBodyBindingSourceAutomaticValidation = true;

            // Enable validation for parameters bound from the `BindingSource.Query` binding source.
            configuration.EnableQueryBindingSourceAutomaticValidation = true;
        });
    }

    public static IServiceCollection ConfigureAppServices(this IServiceCollection services)
    {
        return services
            // Testing/Development services
            .AddScoped<TestDataService>()
            // Auth/Users services:
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IAuthHelper, AuthHelper>()
            .AddScoped<IUsersService, UsersService>()
            // General purpose services:
            .AddScoped<IEntityExistenceChecker, EntityExistenceChecker>()
            // University services:
            .AddScoped<IClassroomsService, ClassroomsService>()
            .AddScoped<IGradesService, GradesService>()
            .AddScoped<IAssignedGradesService, AssignedGradesService>()
            .AddScoped<ICoursesService, CoursesService>()
            .AddScoped<IGroupsService, GroupsService>()
            .AddScoped<IStudentGroupsService, StudentGroupsService>()
            .AddScoped<ISemestersService, SemestersService>()
            .AddScoped<IStudentSemestersService, StudentSemestersService>()
            .AddScoped<IClassesService, ClassesService>()
            .AddScoped<IClassTypesService, ClassTypesService>()
            .AddScoped<IActivityTypesService, ActivityTypesService>();
    }

    public static IMvcBuilder ConfigureControllers(this IServiceCollection builder)
    {
        return builder
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            })
            .ConfigureApiBehaviorOptions(x => { x.SuppressMapClientErrors = true; });
    }
}
