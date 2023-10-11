namespace EUniversity.Core.Models.University
{
    public class Classroom
    {
        public const int MaxNameLength = 50;

        [Key]
        public int Id { get; set; }
        [StringLength(MaxNameLength)]
        public string Name { get; set; } = null!;
    }
}
