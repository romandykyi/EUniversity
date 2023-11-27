using EUniversity.Core.Models;
using EUniversity.Infrastructure.Filters;

namespace EUniversity.Tests.Filters;

public class UsersFilterTests
{
    [Test]
    public void EmptyFilter_ReturnsEntireQuery()
    {
        // Arrange
        UsersFilterProperties properties = new();
        UsersFilter filter = new(properties);
        ApplicationUser[] array =
        {
            new ApplicationUser() { Id = "1", Email = "email1@example.com", FirstName = "Walter", LastName = "White"},
            new ApplicationUser() { Id = "2", Email = "email2@example.com", FirstName = "Jesse", LastName = "Pinkman"},
            new ApplicationUser() { Id = "3", Email = "email3@example.com", FirstName = "Tuco", LastName = "Salamanca"}
        };

        // Act
        var result = filter.Apply(array.AsQueryable());

        // Assert
        Assert.That(result, Is.EquivalentTo(array));
    }

    [Test]
    public void FullNameSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        UsersFilterProperties properties = new(FullName: "aa");
        UsersFilter filter = new(properties);
        ApplicationUser[] array =
        {
            new ApplicationUser() { Id = "1", Email = "email1@example.com", FirstName = "Aaa", LastName = "Bbbb"},
            new ApplicationUser() { Id = "2", Email = "email2@example.com", FirstName = "Cccc", MiddleName="Aapaa", LastName = "Dddd"},
            new ApplicationUser() { Id = "3", Email = "email3@example.com", FirstName = "Gggg", LastName = "Hhhh"},
            new ApplicationUser() { Id = "4", Email = "email4@example.com", FirstName = "Eeee", MiddleName="Bbbb", LastName = "Baaca"}
        };
        string[] expectedUsersIds = { "1", "2", "4" };

        // Act
        var result = filter.Apply(array.AsQueryable());

        // Assert
        Assert.That(result.Select(u => u.Id), Is.EquivalentTo(expectedUsersIds));
    }

    [Test]
    public void EmailSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        UsersFilterProperties properties = new(Email: "amail.com");
        UsersFilter filter = new(properties);
        ApplicationUser[] array =
        {
            new ApplicationUser() { Id = "1", Email = "email11amail.com", FirstName = "Joe", LastName = "Doe"},
            new ApplicationUser() { Id = "2", Email = "email2@example.io", FirstName = "Jane", MiddleName="Diana", LastName = "Doe"},
            new ApplicationUser() { Id = "3", Email = "email13@amail.com", FirstName = "Freya", LastName = "Black"},
            new ApplicationUser() { Id = "4", Email = "email4@example.io", FirstName = "Johny", MiddleName="Little", LastName = "Johnson"}
        };
        string[] expectedUsersIds = { "1", "3" };

        // Act
        var result = filter.Apply(array.AsQueryable());

        // Assert
        Assert.That(result.Select(u => u.Id), Is.EquivalentTo(expectedUsersIds));
    }

    [Test]
    public void UserNameSpecified_ReturnsFilteredQuery()
    {
        // Arrange
        UsersFilterProperties properties = new(UserName: "master");
        UsersFilter filter = new(properties);
        ApplicationUser[] array =
        {
            new ApplicationUser() { Id = "1", UserName="ilovecheese"},
            new ApplicationUser() { Id = "2", UserName="noobmaster69"},
            new ApplicationUser() { Id = "3", UserName="carrot_eater"},
            new ApplicationUser() { Id = "4", UserName="dungeon_master"}
        };
        string[] expectedUsersIds = { "2", "4" };

        // Act
        var result = filter.Apply(array.AsQueryable());

        // Assert
        Assert.That(result.Select(u => u.Id), Is.EquivalentTo(expectedUsersIds));
    }
}
