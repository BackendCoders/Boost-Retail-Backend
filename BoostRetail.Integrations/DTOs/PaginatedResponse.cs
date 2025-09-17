namespace BoostRetail.Integrations.SConnect.DTOs
{
    public class PaginatedResponse<T>
    {
        public AttributeInfo Attribute { get; set; }
        public IEnumerable<T> Values { get; set; }
    }

    public class AttributeInfo
    {
        public int Count { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
    }
}
