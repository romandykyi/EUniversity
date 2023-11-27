namespace EUniversity.Core.Models;

/// <summary>
/// An interface that defines an object that has a Name property.
/// </summary>
public interface IHasName
{
    /// <summary>
    /// The name of the object.
    /// </summary>
    string Name { get; }
}
