using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Boost.Shared
{
    public enum Role
    {
        Admin = 1,
        User = 2,
    }

    public class ApplicationUser : IdentityUser
    {
        public Guid? TenantId { get; set; }
        public Tenant? Tenant { get; set; }
    }

    public class Tenant
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string DbName { get; set; }
        public string DbConnectionString { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class AppUserRole : IdentityUserRole<int>
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }

    public class LoginModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
    public class RegisterModel
    {


        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "User Role is required")]
        public Role UserRole { get; set; }
    }
    public class ChangePasswordModel
    {
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
    public class RegisterByPhoneModel
    {
        public string Username { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }

        [Required(ErrorMessage = "User Role is required")]
        public Role UserRole { get; set; }
    }
    public class VerifyPhoneModel
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string OTP { get; set; }
    }
    public class LoginByPhoneRequestModel
    {
        public string PhoneNumber { get; set; }
    }
    public class ResendOtpModel
    {
        public string PhoneNumber { get; set; }
    }
    public class ExternalLoginModel
    {
        public string Provider { get; set; } // Google or Facebook
        public string IdToken { get; set; }  // Google: ID Token | Facebook: Access Token
    }
    public class ExternalLoginPayload
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
    }
}
