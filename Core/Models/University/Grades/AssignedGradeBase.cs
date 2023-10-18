using System.ComponentModel.DataAnnotations.Schema;

namespace EUniversity.Core.Models.University.Grades
{
    public abstract class AssignedGradeBase<TId> : IEntity<TId>
        where TId : IEquatable<TId>
    {
        public const int MaxNotesLength = 500;

        [Key]
        public TId Id { get; set; } = default!;
        [ForeignKey(nameof(Grade))]
        public int GradeId { get; set; }
        [ForeignKey(nameof(Teacher))]
        public string? TeacherId { get; set; }
        [ForeignKey(nameof(Student))]
        public string? StudentId { get; set; }
        public DateTimeOffset Date { get; set; }
        public string? Notes { get; set; }

        public Grade Grade { get; set; } = null!;
        public ApplicationUser? Teacher { get; set; }
        public ApplicationUser? Student { get; set; }
    }
}
