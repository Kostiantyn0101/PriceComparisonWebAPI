namespace Domain.Models.Primitives
{
    public class CompositeKey<TKey1, TKey2>
    {
        public TKey1 Key1 { get; set; }
        public TKey2 Key2 { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is CompositeKey<TKey1, TKey2> other)
            {
                return EqualityComparer<TKey1>.Default.Equals(Key1, other.Key1) &&
                       EqualityComparer<TKey2>.Default.Equals(Key2, other.Key2);
            }
            return false;
        }

        public override int GetHashCode() => HashCode.Combine(Key1, Key2);
    }
}
