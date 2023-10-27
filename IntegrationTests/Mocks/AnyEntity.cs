using EUniversity.Core.Models;
using NSubstitute;

namespace EUniversity.IntegrationTests.Mocks;

/// <summary>
/// A type that can be used to match generic parameter with 
/// <see cref="IEntity{T}"/> constraints.
/// </summary>
public class AnyEntity : Arg.AnyType, IEntity<AnyEntityId>
{
    public AnyEntityId Id { get; set; } = null!;
}
