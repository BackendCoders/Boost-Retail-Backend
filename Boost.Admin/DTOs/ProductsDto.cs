namespace Boost.Admin.DTOs
{
    public class SIMProductsDto
    {
        public List<string> NotFound { get; set; } = new();
        public List<SIMProductDto> Products { get; set; }
    }
}
