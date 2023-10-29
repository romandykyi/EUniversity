namespace EUniversity.Core.Services.University;

/// <summary>
/// Represents the possible results of add/remove 
/// operations involving student-group associations.
/// </summary>
public enum StudentGroupResult
{
    /// <summary>
    /// The operation was successful.
    /// </summary>
    Success,
    /// <summary>
    /// The student associated with the operation was not found.
    /// </summary>
    StudentNotFound,
    /// <summary>
    /// The group associated with the operation was not found.
    /// </summary>
    GroupNotFound
}
