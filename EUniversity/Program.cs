using EUniversity.Core.Mapping;
using EUniversity.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.ConnectDatabase();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAntiforgery(options => options.HeaderName = "X-XSRF-Token");

builder.Services.ConfigureAppServices();

builder.Services.AddCustomizedIdentity();
builder.Services.AddCustomizedAuthorization();

builder.Services.ConfigureControllers();
builder.Services.AddFluentValidation();
MappingGlobalSettings.Apply();

builder.Services.AddSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();

	app.UseMigrationsEndPoint();
}
else
{
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCookiePolicy();
app.UseIdentityServer();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.CreateRoles();
app.CreateAdministrator();
if (app.Environment.IsDevelopment())
{
	app.CreateTestUsers();
}

app.Run();

public partial class Program { }
