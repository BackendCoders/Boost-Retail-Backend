
using Boost.Retail.Domain.Enums;

namespace Boost.Retail.Data.Models
{
    public class TillPaymentType : BaseEntity
    {
        public PaymentType Type { get; set; }
        public string Name { get; set; }
    }
}
