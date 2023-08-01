using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySchool.Core.Interface;
using SavingsApp.Data.Entities.DTOs.Request;
using SavingsApp.Data.Entities.DTOs.Response;
using SavingsApp.Data.Entities.Models;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Savings_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IMapper mapper, IEmailService emailService, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
            _emailService = emailService;
            _signInManager = signInManager;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] SignUpDto regRequest, bool enableTwoFactorAuthentication = false)
        {
            // Check if user exists
            var userExist = await _userManager.FindByEmailAsync(regRequest.Email);
            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new APIResponse { StatusCode = "Error", IsSuccess = false, Message = "User already exists!" });
            }

            // Add the user to the database
            // ApplicationUser user = _mapper.Map<ApplicationUser>(regRequest);
            //Create ApplicationUser
            ApplicationUser user = new ApplicationUser()
            {
                UserName = regRequest.Email,
                Email = regRequest.Email,
                FirstName = regRequest.FirstName,
                LastName = regRequest.LastName,
                PhoneNumber = regRequest.PhoneNumber,

            };

            var result = await _userManager.CreateAsync(user, regRequest.Password);

            if (result.Succeeded)
            {

                // Enable or disable two-factor authentication based on the parameter
                if (enableTwoFactorAuthentication)
                {
                    await _userManager.SetTwoFactorEnabledAsync(user, true);
                }


                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { token, email = user.Email }, Request.Scheme);
                var message = new Message(new string[] { user.Email! }, "Confirmation Email Link", $"Click on the link to confirm your email: {confirmationLink!}");
                _emailService.SendEmail(message);

                return StatusCode(StatusCodes.Status200OK,
                    new APIResponse { StatusCode = "Success", IsSuccess = true, Message = $"User created successfully. Check your email {user.Email} for a confirmation link!" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new APIResponse { StatusCode = "Error", IsSuccess = false, Message = "Failed to create user!" });
            }
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            //check if user exist
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                       new APIResponse { StatusCode = "Error", IsSuccess = false, Message = "User does not exist" });
            }
            if (user.TwoFactorEnabled)
            {
                await _signInManager.SignOutAsync();
                await _signInManager.PasswordSignInAsync(user, loginRequest.Password, false, true);
                var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                var message = new Message(new string[] { user.Email! }, "OTP Confirmation", $"Your token is {token!} ");
                _emailService.SendEmail(message);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new APIResponse { StatusCode = "Success", IsSuccess = true, Message = "OTP sent to your Email" });

            }
            if (user != null && await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                var authClaims = new List<Claim>
                 {
                     new Claim(ClaimTypes.Name, user.UserName),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 };
                var JwtToken = GetToken(authClaims);
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(JwtToken),
                    expiration = JwtToken.ValidTo
                });

            }
            return Unauthorized();
        }


        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK,
                       new APIResponse { StatusCode = "Success", IsSuccess = true, Message = "Email Verified!" });
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError,
                        new APIResponse { StatusCode = "Error", IsSuccess = false, Message = "User does not exist" });
        }


        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword([Required] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var forgotPasswordLink = Url.Action(nameof(ResetPassword), "Auth", new { token, email = user.Email }, Request.Scheme);
                var message = new Message(new string[] { user.Email! }, "Forgot password link", $"Click on the link {forgotPasswordLink!} to reset your password ");
                _emailService.SendEmail(message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new APIResponse { StatusCode = "Success", IsSuccess = true, Message = $"Password reset link sent to {user.Email} successfully" });
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                        new APIResponse { StatusCode = "Error", IsSuccess = false, Message = "Reset password fail! Please verify your email and try again." });
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var passwordReset = new PasswordReset { Token = token, Email = email };

            return Ok(new
            {
                passwordReset
            });
        }


        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(PasswordReset resetpassword)
        {
            var user = await _userManager.FindByEmailAsync(resetpassword.Email);
            if (user != null)
            {
                var passwordreset = await _userManager.ResetPasswordAsync(user, resetpassword.Token, resetpassword.Password);
                if (!passwordreset.Succeeded)
                {
                    foreach (var error in passwordreset.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return Ok(ModelState);
                }
                return StatusCode(StatusCodes.Status200OK,
                       new APIResponse { StatusCode = "Success", IsSuccess = true, Message = "Password changed successfully!" });
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                        new APIResponse { StatusCode = "Error", IsSuccess = false, Message = "Reset password failed!" });
        }
    }
}
