using System.ComponentModel.DataAnnotations.Schema;

namespace EUniversity.Core.Models.University
{
    public class StudentGroup : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Student))]
        public string StudentId { get; set; } = null!;
        [ForeignKey(nameof(Group))]
        public int GroupId { get; set; }

        public ApplicationUser Student { get; set; } = null!;
        public Group Group { get; set; } = null!;
    }
}
