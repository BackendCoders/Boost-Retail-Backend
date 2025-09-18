using Azure;
using Boost.Admin.Data;
using Boost.Admin.Data.Models;
using Boost.Retail.Data;
using Boost.Retail.Services.Interfaces;
using Boost.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace BoostRetailAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly ITenantDbContextFactory _tenantDbContextFactory;

        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IEmailSender emailSender, ITenantDbContextFactory tenantDbContextFactory)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _tenantDbContextFactory = tenantDbContextFactory;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Username);
            if (user == null)
            {
                user = await userManager.FindByNameAsync(model.Username);
            }

            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Secret"]));

                var token = new JwtSecurityToken(
                    expires: DateTime.UtcNow.AddDays(int.Parse(_configuration["JwtConfig:ExpiryDays"])),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    userid = user.Id,
                    role = userRoles.FirstOrDefault(), 
                    email = user.Email,
                    username = user.UserName,
                    emailConfirmed = user.EmailConfirmed,
                    phone = user.PhoneNumber,
                    phoneConfirmed = user.PhoneNumberConfirmed,
                    twoFactorEnabled = user.TwoFactorEnabled,
                    tenantId = user.TenantId
                });
            }
            return Unauthorized();
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);

            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Boost.Admin.Data.Models.Response { Status = "Error", Message = "User already exists!" });
            
            var tenantId = Guid.NewGuid();
            var tenant = new Tenant
            {
                Id = tenantId,
                UserName = model.Username,
                CreatedOn = DateTime.UtcNow
            };

            if (model.UserRole == Role.User)
            {
               
                //var dbName = $"BOOST_{tenantId}";
                //var connectionString = $"Server=.;Database={dbName};Persist Security Info=False;User ID=sa;Password=Polopolo121;Encrypt=False;Trusted_Connection=False;TrustServerCertificate=True;";

                //// Create tenant DB
                //var createDbQuery = $"CREATE DATABASE [{dbName}]";
                //using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                //{
                //    await conn.OpenAsync();
                //    using var cmd = new SqlCommand(createDbQuery, conn);
                //    await cmd.ExecuteNonQueryAsync();
                //}

                //// Optionally run migrations or schema setup for new DB
                //var tenantOptions = new DbContextOptionsBuilder<BoostDbContext>()
                //    .UseSqlServer(connectionString).Options;

                //using (var tenantContext = new BoostDbContext(tenantOptions))
                //{
                //    await tenantContext.Database.MigrateAsync(); // if using EF Migrations
                //}

                //// Save tenant in Master DB
                //tenant.DbName = dbName;
                //tenant.DbConnectionString = connectionString;

                await _tenantDbContextFactory.CreateTenant(tenant);
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Tenant = model.UserRole == Role.User ? tenant : null,
                TenantId = model.UserRole == Role.User ? tenantId : null,
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new { status = "Error", message = $"User creation failed! {errors}" });
            }


            if (!await roleManager.RoleExistsAsync(AppUserRole.Admin))
                await roleManager.CreateAsync(new IdentityRole(AppUserRole.Admin));
            if (!await roleManager.RoleExistsAsync(AppUserRole.User))
                await roleManager.CreateAsync(new IdentityRole(AppUserRole.User));

            if (model.UserRole == Role.Admin)
            {
                if (await roleManager.RoleExistsAsync(AppUserRole.Admin))
                {
                    await userManager.AddToRoleAsync(user, AppUserRole.Admin);
                }
            }
            else if (model.UserRole == Role.User)
            {
                if (await roleManager.RoleExistsAsync(AppUserRole.User))
                {
                    await userManager.AddToRoleAsync(user, AppUserRole.User);
                }
            }

            // Generate email confirmation token
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            // Create the confirmation link
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authenticate", new { token, email = user.Email }, Request.Scheme);

            // Send email (assuming you have a service for sending emails)
            await _emailSender.SendEmailAsync(user.Email, "Confirm your email for SIM registration",
                $"Please confirm your account by clicking <a href='{confirmationLink}'>here</a>. <br> Thank you for signing up.");


            return Ok(new Boost.Admin.Data.Models.Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpGet]
        [Route("confirmemail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest(new Boost.Admin.Data.Models.Response { Status = "Error", Message = "Invalid email address." });

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Boost.Admin.Data.Models.Response { Status = "Error", Message = "Email confirmation failed!" });


            var userRoles = await userManager.GetRolesAsync(user);
            var primaryRole = userRoles.FirstOrDefault();

            return Ok(new Boost.Admin.Data.Models.Response { Status = "Success", Message = "Email confirmed successfully!" });
        }

        [HttpPost]
        [Route("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Username);
            if (user == null)
            {
                return NotFound(new { status = "Error", message = "User not found!" });
            }

            var result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new { status = "Error", message = $"Password change failed! {errors}" });
            }

            return Ok(new { status = "Success", message = "Password changed successfully!" });
        }

    }
}
