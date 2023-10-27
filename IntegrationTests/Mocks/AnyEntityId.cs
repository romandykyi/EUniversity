using EUniversity.Core.Models;
using NSubstitute;

namespace EUniversity.IntegrationTests.Mocks;

/// <summary>
/// A type that can be used to match generic parameter with 
/// <see cref="IEntity{T}.Id"/> constraints.
/// </summary>
public class AnyEntityId : Arg.AnyType, IEquatable<AnyEntityId>
{
    public bool Equals(AnyEntityId? other)
    {
        return true;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as AnyEntityId);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
