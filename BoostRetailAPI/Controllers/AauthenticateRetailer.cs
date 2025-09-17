using Boost.Retail.Data.Models;
using Boost.Retail.Domain.Enums;
using Boost.Retail.Services.Interfaces;
using Boost.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace BoostRetailAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AuthenticateRetailController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly ISMSSender _smsSender;
      

        //private readonly INotificationFCMLogic _notificationFCMLogic;

        public AuthenticateRetailController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IEmailSender emailsender, ISMSSender smssender)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _emailSender = emailsender;
            _smsSender = smssender;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Username);
            if(user == null) 
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
                    fullname = user.FullName,
                    email = user.Email,
                    emailConfirmed = user.EmailConfirmed,
                    phone = user.PhoneNumber,
                    phoneConfirmed = user.PhoneNumberConfirmed,
                    twoFactorEnabled = user.TwoFactorEnabled,
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
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            AppUser user = new AppUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
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
            await _emailSender.SendEmailAsync(user.Email, "Confirm your email for EPOS registration",
                $"Please confirm your account by clicking <a href='{confirmationLink}'>here</a>. <br> Thank you for signing up.");


            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpGet]
        [Route("confirmemail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest(new Response { Status = "Error", Message = "Invalid email address." });

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Email confirmation failed!" });


            var userRoles = await userManager.GetRolesAsync(user);
            var primaryRole = userRoles.FirstOrDefault();
            
            return Ok(new Response { Status = "Success", Message = "Email confirmed successfully!" });
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


        [HttpPost]
        [Route("registerbyphone")]
        public async Task<IActionResult> RegisterByPhone([FromBody] RegisterByPhoneModel model)
        {
            var userExists = await userManager.Users.AnyAsync(u => u.PhoneNumber == model.PhoneNumber);
            if (userExists)
                return BadRequest(new { status = "Error", message = "Phone number already registered!" });

            var user = new AppUser
            {
                UserName = model.Username,
                Email = model.Username,
                PhoneNumber = model.PhoneNumber
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
           
            // Generate and send OTP for phone verification
            var otp = new Random().Next(100000, 999999).ToString();
            await userManager.SetAuthenticationTokenAsync(user, "PhoneVerification", "OTP", otp);
            //Console.WriteLine($"OTP for {user.PhoneNumber}: {otp}"); // Replace with actual SMS sending
            await _smsSender.SendSMSAsync(user.PhoneNumber, $"{otp} is OTP for your EPOS Registration. Thank you for signing up." );
            return Ok(new { status = "Success", message = "User registered! OTP sent for verification." });
        }

        [HttpPost]
        [Route("verifyphone")]
        public async Task<IActionResult> VerifyPhone([FromBody] VerifyPhoneModel model)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber);
            if (user == null)
                return NotFound(new { status = "Error", message = "User not found!" });

            var storedOtp = await userManager.GetAuthenticationTokenAsync(user, "PhoneVerification", "OTP");
            if (storedOtp == null || storedOtp != model.OTP)
                return BadRequest(new { status = "Error", message = "Invalid OTP!" });

            user.PhoneNumberConfirmed = true;
            await userManager.UpdateAsync(user);

            var userRoles = await userManager.GetRolesAsync(user);
            var primaryRole = userRoles.FirstOrDefault();
                        

            // After successful verification, you should likely remove the used OTP
            await userManager.RemoveAuthenticationTokenAsync(user, "PhoneVerification", "OTP");


            //return Ok(new { status = "Success", message = "Phone number verified successfully!" });

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
                fullname = user.FullName,
                email = user.Email,
                emailConfirmed = user.EmailConfirmed,
                phone = user.PhoneNumber,
                phoneConfirmed = user.PhoneNumberConfirmed,
                twoFactorEnabled = user.TwoFactorEnabled,
            });
        }

        [HttpPost]
        [Route("resendotp")]
        public async Task<IActionResult> ResendOTP([FromBody] ResendOtpModel model)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber);
            if (user == null)
                return NotFound(new { status = "Error", message = "User not found!" });

            if (user.PhoneNumberConfirmed)
                return BadRequest(new { status = "Error", message = "Phone number already verified." });

            var otp = new Random().Next(100000, 999999).ToString();

            await userManager.SetAuthenticationTokenAsync(user, "PhoneVerification", "OTP", otp);
            await _smsSender.SendSMSAsync(user.PhoneNumber, $"{otp} is your EPOS OTP. Please use it to verify your phone number.");

            return Ok(new { status = "Success", message = "OTP resent successfully." });
        }

        [HttpPost]
        [Route("loginbyphone")]
        public async Task<IActionResult> LoginByPhoneRequestOTP([FromBody] LoginByPhoneRequestModel model)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber);

            if (user == null)
            {
                return NotFound(new { status = "Error", message = "Phone number not registered." });
            }

            if (!user.PhoneNumberConfirmed)
            {
                return Unauthorized(new { status = "Error", message = "Phone number not verified. Please verify your phone number first." });
            }

            // Generate a new OTP for login
            var otp = new Random().Next(100000, 999999).ToString();

            // Store the OTP associated with the user for login verification
            await userManager.SetAuthenticationTokenAsync(user, "PhoneVerification", "OTP", otp);

            // Send the OTP to the user's phone number
            await _smsSender.SendSMSAsync(user.PhoneNumber, $"{otp} is your EPOS OTP for login.");

            return Ok(new { status = "Success", message = "OTP sent to your phone number for login." });
        }


        //[HttpPost]
        //[Route("externallogin")]
        //public async Task<IActionResult> ExternalLogin([FromBody] ExternalLoginModel model)
        //{
        //    var payload = await VerifyExternalToken(model);

        //    if (payload == null)
        //        return BadRequest(new { status = "Error", message = "Invalid external token" });

        //    var info = new UserLoginInfo(model.Provider, payload.Subject, model.Provider);
        //    var user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
        //    var externaluser = await userManager.FindByEmailAsync(payload.Email);

        //    if (user == null || externaluser == null)
        //    {
        //        if (externaluser == null)
        //        {
        //            externaluser = new AppUser
        //            {
        //                Email = payload.Email,
        //                UserName = payload.Email,
        //                EmailConfirmed = true,
        //                FullName = payload.Name
        //            };

        //            var result = await userManager.CreateAsync(externaluser);
        //            if (!result.Succeeded)
        //                return BadRequest(new { status = "Error", message = "User creation failed!" });

        //            if (await roleManager.RoleExistsAsync(AppUserRole.User))
        //                await userManager.AddToRoleAsync(externaluser, AppUserRole.User);
        //        }
        //        if (user == null)
        //        {
        //            user = externaluser;
        //            await userManager.AddLoginAsync(user, info);
        //        }
        //    }
            
        //    var userRoles = await userManager.GetRolesAsync(user);
        //    var authClaims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, user.UserName),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //    };

        //    foreach (var userRole in userRoles)
        //        authClaims.Add(new Claim(ClaimTypes.Role, userRole));

        //    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Secret"]));

        //    var token = new JwtSecurityToken(
        //        expires: DateTime.UtcNow.AddDays(int.Parse(_configuration["JwtConfig:ExpiryDays"])),
        //        claims: authClaims,
        //        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        //    );

        //    return Ok(new
        //    {
        //        token = new JwtSecurityTokenHandler().WriteToken(token),
        //        expiration = token.ValidTo,
        //        userid = user.Id,
        //        role = userRoles.FirstOrDefault(),
        //        fullname = user.FullName,
        //        email = user.Email,
        //        emailConfirmed = user.EmailConfirmed,
        //        phone = user.PhoneNumber,
        //        phoneConfirmed = user.PhoneNumberConfirmed,
        //        twoFactorEnabled = user.TwoFactorEnabled,
        //    });
        //}

        //private async Task<ExternalLoginPayload> VerifyExternalToken(ExternalLoginModel model)
        //{
        //    if (model.Provider == "Google")
        //    {
        //        var settings = new GoogleJsonWebSignature.ValidationSettings()
        //        {
        //            Audience = new[] { _configuration["Authentication:Google:ClientId"] }
        //        };

        //        var payload = await GoogleJsonWebSignature.ValidateAsync(model.IdToken, settings);
        //        return new ExternalLoginPayload
        //        {
        //            Email = payload.Email,
        //            Name = payload.Name,
        //            Subject = payload.Subject
        //        };
        //    }
        //    else if (model.Provider == "Facebook")
        //    {
        //        using var httpClient = new HttpClient();
        //        var fbUrl = $"https://graph.facebook.com/me?fields=email,name&access_token={model.IdToken}";
        //        var response = await httpClient.GetStringAsync(fbUrl);
        //        var fbInfo = JsonSerializer.Deserialize<JsonElement>(response);

        //        return new ExternalLoginPayload
        //        {
        //            Email = fbInfo.GetProperty("email").GetString(),
        //            Name = fbInfo.GetProperty("name").GetString(),
        //            Subject = fbInfo.GetProperty("id").GetString()
        //        };
        //    }

        //    return null;
        //}



        //[HttpPost]
        //[Route("sendnotification")]
        //public async Task<ActionResult<string>> SendNotification(SendNotificationDto request)
        //{
        //    return await _notificationFCMLogic.SendNotification(request);
        //}

        //[HttpGet]
        //[Route("getFCM")]
        //public ActionResult<NotificationFCM> GetFCM(string userId)
        //{
        //    return _notificationFCMLogic.GetFCM(userId);
        //}

        //[HttpPost]
        //[Route("updateFCM")]
        //public ActionResult<bool> UpdateFCM(string userId, string FCM)
        //{
        //    return _notificationFCMLogic.UpdateFCM(userId, FCM);
        //}
    }
}
