namespace EUniversity.Core.Models
{
    public interface IEntity<T> where T : IEquatable<T>
    {
        public T Id { get; set; }
    }
}
