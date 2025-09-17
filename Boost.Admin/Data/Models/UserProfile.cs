using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Boost.Admin.Data.Models
{
    public class UserProfile
    {
        [Key]
        public int UserId { get; set; }

        [Precision(18,2)]
        public decimal ReduceRRPPercent { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual IdentityUser User { get; set; }
    }
}
