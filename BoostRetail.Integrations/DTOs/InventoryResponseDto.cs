namespace BoostRetail.Integrations.SConnect.DTOs
{
    public class InventoryResponseDto
    {
        public string Description { get; set; }
        public string ManufacturerSku { get; set; }
        public string Manufacturer { get; set; }
        public string Upc { get; set; }
        public string Ean { get; set; }
        public string CustomSku { get; set; }
        public string Category { get; set; }
        public string ModelYear { get; set; }
        public decimal AvgCost { get; set; }
        public decimal SellingPrice { get; set; }
        public int Qoh { get; set; }
        public bool Serialized { get; set; }
        public string ItemId { get; set; }
        public string ShopSymbol { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}
