namespace Boost.Admin.DTOs
{
    public class DescriptionsDto
    {
        public List<string> NotFound { get; set; } = new();
        public List<DescriptionDto> Descriptions { get; set; }
    }
}
