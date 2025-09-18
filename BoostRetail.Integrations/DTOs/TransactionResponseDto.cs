namespace BoostRetail.Integrations.SConnect.DTOs
{
    public class TransactionResponseDto
    {
        public string Id { get; set; }
        public bool Completed { get; set; }
        public bool Archived { get; set; }
        public bool Voided { get; set; }
        public string CreateTime { get; set; }
        public string UpdateTime { get; set; }
        public List<TransactionLineDto> Lines { get; set; }
    }
}
