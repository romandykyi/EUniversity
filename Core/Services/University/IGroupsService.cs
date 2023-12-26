using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;

namespace EUniversity.Core.Services.University;

/// <summary>
/// Response of the <see cref="IGroupsService.GetOwnerIdAsync(int)" /> method.
/// </summary>
/// <param name="GroupExists">Flag that determines whether requested group exists.</param>
/// <param name="OwnerId">ID of the owner of the group(<see langword="null" /> if group doesn't exist).</param>
public record GetOwnerIdResponse(bool GroupExists, string? OwnerId);

public interface IGroupsService :
    ICrudService<Group, int, GroupPreviewDto, GroupViewDto, GroupCreateDto, GroupCreateDto>
{
    /// <summary>
    /// Gets an ID of the owner of the group(TeacherId).
    /// </summary>
    /// <param name="groupId">ID of the group which owner will be returned.</param>
    /// <returns>
    /// A task that represents an asynchronous operation containing
    /// the result of the operation.
    /// </returns>
    Task<GetOwnerIdResponse> GetOwnerIdAsync(int groupId);
}
