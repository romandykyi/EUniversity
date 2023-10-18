namespace EUniversity.Core.Models.University
{
    public class Course : IEntity<int>
    {
        public const int MaxNameLength = 200;
        public const int MaxDescriptionLength = 1000;

        [Key]
        public int Id { get; set; }
        [StringLength(MaxNameLength)]
        public string Name { get; set; } = null!;
        [StringLength(MaxDescriptionLength)]
        public string Description { get; set; } = null!;
    }
}
