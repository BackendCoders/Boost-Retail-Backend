namespace Boost.Admin.DTOs
{
    public class ImagesDto
    {
        public List<string> NotFound { get; set; } = new();
        public List<ImageDto> Images { get; set; }
    }
}
