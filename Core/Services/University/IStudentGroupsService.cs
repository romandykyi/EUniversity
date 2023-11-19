using EUniversity.Core.Models.University;

namespace EUniversity.Core.Services.University;

/// <summary>
/// Represents an interface of the service which configures
/// the 'Students->Groups' many-to-many relationship.
/// </summary>
public interface IStudentGroupsService : IAssigningService<StudentGroup, string, int>
{
}
