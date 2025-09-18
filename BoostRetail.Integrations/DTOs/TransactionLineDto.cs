namespace BoostRetail.Integrations.SConnect.DTOs
{
    public class TransactionLineDto
    {
        public int Id { get; set; }
        public string ItemId { get; set; }
        public decimal CalcTotal { get; set; }
        public string CustomerId { get; set; }
        public int UnitQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal CalcLineDiscount { get; set; }
        public decimal Tax2Rate { get; set; }
        public string Description { get; set; }
        public string Sku { get; set; }
        public string Upc { get; set; }
        public string Ean { get; set; }
        public string CustomSku { get; set; }
        public string CreateTime { get; set; }
        public string UpdateTime { get; set; }
        public string Timezone { get; set; }
    }
}
