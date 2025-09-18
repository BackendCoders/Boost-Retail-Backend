namespace Boost.Admin.DTOs
{
    public class SpecDto
    {
        public string Text { get; set; }
        public List<KeyValueDto> Specs { get; set; } = new();
    }
}
