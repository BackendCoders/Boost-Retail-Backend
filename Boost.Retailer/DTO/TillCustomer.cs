using Boost.Retail.Data.Models;

namespace Boost.Retail.Data.DTO
{
    public class TillCustomer
    {
        public string CustomerAccount { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Details { get; set; }
        public string Category { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string LoyaltyCardNumber { get; set; }
        public string ClaimRef { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public bool DeliveryGoods { get; set; }
        public decimal CreditLimit { get; set; } = 0.00M;
        public List<Layaway> Layaways { get; set; }
    }


}
