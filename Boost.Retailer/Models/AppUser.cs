using Boost.Retail.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Boost.Retail.Data.Models
{
    public class AppUser : IdentityUser
    {
        [MinLength(3), MaxLength(50)]
        public string? FullName { get; set; }
    }
   
}
