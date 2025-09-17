namespace Boost.Admin.DTOs
{
    public class BarcodesDto
    {
        public List<string> NotFound { get; set; } = new();
        public List<BarcodeDto> Barcodes { get; set; } 
    }
}
