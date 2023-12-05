using EUniversity.Core.Models;
using EUniversity.Core.Services;
using EUniversity.Core.Services.Auth;
using EUniversity.Core.Services.University;
using EUniversity.Core.Services.Users;
using EUniversity.Extensions;
using EUniversity.Infrastructure.Data;
using EUniversity.Infrastructure.Services.University;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System.Net.Http.Headers;

namespace EUniversity.IntegrationTests.Mocks;

/// <summary>
/// <see cref="WebApplicationFactory{}" /> for testing with mocked services and authentication.
/// </summary>
public class MockedProgramWebApplicationFactory : WebApplicationFactory<Program>
{
    public TestClaimsProvider ClaimsProvider { get; private set; } = null!;
    public IAuthService AuthServiceMock { get; private set; } = null!;
    public UserManager<ApplicationUser> UserManagerMock { get; private set; } = null!;
    public IUsersService UsersServiceMock { get; private set; } = null!;

    public IEntityExistenceChecker ExistenceCheckerMock { get; private set; } = null!;

    public IClassroomsService ClassroomsServiceMock { get; private set; } = null!;
    public IGradesService GradesServiceMock { get; private set; } = null!;
    public ICoursesService CoursesServiceMock { get; private set; } = null!;
    public IGroupsService GroupsServiceMock { get; private set; } = null!;
    public IClassesService ClassesServiceMock { get; private set; } = null!;
    public IStudentGroupsService StudentGroupsServiceMock { get; private set; } = null!;
    public ISemestersService SemestersServiceMock { get; private set; } = null!;
    public IStudentSemestersService StudentSemestersServiceMock { get; private set; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ClaimsProvider = new();
        AuthServiceMock = Substitute.For<IAuthService>();

        var mockedUserStore = Substitute.For<IUserStore<ApplicationUser>>();
        UserManagerMock = Substitute.For<UserManager<ApplicationUser>>(
            mockedUserStore, null, null, null, null, null, null, null, null
            );
        UsersServiceMock = Substitute.For<IUsersService>();

        ExistenceCheckerMock = Substitute.For<IEntityExistenceChecker>();

        ClassroomsServiceMock = Substitute.For<IClassroomsService>();
        GradesServiceMock = Substitute.For<IGradesService>();
        CoursesServiceMock = Substitute.For<ICoursesService>();
        GroupsServiceMock = Substitute.For<IGroupsService>();
        ClassesServiceMock = Substitute.For<IClassesService>();
        StudentGroupsServiceMock = Substitute.For<IStudentGroupsService>();
        SemestersServiceMock = Substitute.For<ISemestersService>();
        StudentSemestersServiceMock = Substitute.For<IStudentSemestersService>();

        builder.ConfigureTestServices(services =>
        {
            // DB
            services.AddDbContext<ApplicationDbContext>(o => o.UseInMemoryDatabase("Endpoints tests DB")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));

            // Auth
            services.AddScoped(_ => ClaimsProvider);
            services.AddAuthentication(defaultScheme: "TestScheme")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    "TestScheme", options => { });
            services.AddCustomizedAuthorization("TestScheme");
            services.AddScoped(_ => AuthServiceMock);

            // Users
            services.AddScoped(_ => UserManagerMock);
            services.AddScoped(_ => UsersServiceMock);

            // General-purpose
            services.AddScoped(_ => ExistenceCheckerMock);

            // University
            services.AddScoped(_ => ClassroomsServiceMock);
            services.AddScoped(_ => GradesServiceMock);
            services.AddScoped(_ => CoursesServiceMock);
            services.AddScoped(_ => GroupsServiceMock);
            services.AddScoped(_ => ClassesServiceMock);
            services.AddScoped(_ => StudentGroupsServiceMock);
            services.AddScoped(_ => SemestersServiceMock);
            services.AddScoped(_ => StudentSemestersServiceMock);
        });
    }

    public HttpClient CreateCustomClient()
    {
        var client = CreateClient(new()
        {
            AllowAutoRedirect = false
        });

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");

        return client;
    }
}
