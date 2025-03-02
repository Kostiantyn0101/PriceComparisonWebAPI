namespace Domain.Models.Primitives
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
