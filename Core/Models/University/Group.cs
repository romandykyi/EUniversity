using System.ComponentModel.DataAnnotations.Schema;

namespace EUniversity.Core.Models.University;

public class Group : IEntity<int>
{
    public const int MaxNameLength = 50;

    [Key]
    public int Id { get; set; }
    [StringLength(MaxNameLength)]
    public string Name { get; set; } = null!;
    [ForeignKey(nameof(Course))]
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
    [ForeignKey(nameof(Teacher))]
    public string? TeacherId { get; set; }
    public ApplicationUser? Teacher { get; set; }

    public ICollection<ApplicationUser> Students { get; set; } = null!;
}
