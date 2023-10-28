using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Savings_App.Extensions.Util;
using SavingsApp.Core.Services.Interfaces;
using SavingsApp.Data.Entities.DTOs.Request.Auth;
using SavingsApp.Data.Entities.DTOs.Response;
using SavingsApp.Data.Entities.Models;
using SavingsApp.Data.Repositories.IRepositories;
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
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, IMapper mapper, IEmailService emailService,
            SignInManager<ApplicationUser> signInManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
            _emailService = emailService;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Registers a new user. 
        /// </summary>
        /// <param name="input">A DTO containing the user data.</param>
        /// <returns>A 201 - Created Status Code in case of success.</returns>
        /// <response code="201">User has been registered</response>                  
        /// <response code="403">User Already Exist</response>                
        /// <response code="500">Failed to create user!</response>  
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
                Wallet userWallet = new Wallet();
                var ID = userWallet.SetWalletId(regRequest.PhoneNumber);
                userWallet.applicationUser = user;
                userWallet.userId = user.Email;
                userWallet.Balance = 0.00;
                userWallet.CreatedAt = DateTime.UtcNow;
                userWallet.WalletId = ID;

                // Save the Wallet to the database
                _unitOfWork.WalletRepository.Add(userWallet);
                _unitOfWork.Save();

                // Enable or disable two-factor authentication based on the parameter
                if (enableTwoFactorAuthentication)
                {
                    await _userManager.SetTwoFactorEnabledAsync(user, true);
                }


                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { token, email = user.Email }, Request.Scheme);
               // var message = new Message(new string[] { user.Email! }, "Confirmation Email Link", $"Click on the link to confirm your email: {confirmationLink!}");
                var messageBody = EmailTemplateProvider.GenerateConfirmationEmail($"{user.FirstName} {user.LastName}", confirmationLink);
                var message = new Message(new string[] { user.Email! }, "Email Confirmation Link", messageBody);
                _emailService.SendEmail(message);

                return StatusCode(StatusCodes.Status201Created,
                    new APIResponse { StatusCode = "Success", IsSuccess = true, Message = $"User created successfully. Check your email {user.Email} for a confirmation link!" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new APIResponse { StatusCode = "Error", IsSuccess = false, Message = "Failed to create user!" });
            }
        }



        /// <summary>
        /// Performs a user email login. 
        /// </summary>
        /// <param name="input">A DTO containing the user's credentials.</param>
        /// <returns>The Bearer Token (in JWT format).</returns>
        /// <response code="200">User has been logged in</response> 
        /// <response code="202">OTP sent to user's Email</response>   
        /// <response code="401">Login failed (unauthorized)</response>
        /// <response code="500">User does not exist (unauthorized)</response>
        [HttpPost("Login")]
        [ResponseCache(CacheProfileName = "NoCache")]
        public async Task<IActionResult> Login( LoginRequestDTO loginRequest)
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
                var messageBody = EmailTemplateProvider.OTP($"{user.FirstName} {user.LastName}", token);
                var message = new Message(new string[] { user.Email! }, "Login Token", messageBody);
               // var message = new Message(new string[] { user.Email! }, "OTP Confirmation", $"Your token is {token!} ");
                _emailService.SendEmail(message);

                return StatusCode(StatusCodes.Status202Accepted,
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
            return StatusCode(StatusCodes.Status401Unauthorized,
                    new APIResponse { StatusCode = "False", IsSuccess = false, Message = "UNAUTHORIZED" });
        }

        /// <summary>
        /// Confirm Email Address 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="email"></param>
        /// <response code="200">Email Verified</response> 
        /// <response code="400">Validation Error</response> 
        /// <response code="500">User does not exist</response> 
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


        /// <summary>
        /// Send a reset password token to user email
        /// </summary>
        /// <param name="email"></param>
        /// <response code="200">Password reset link sent</response> 
        /// <response code="400">Fail to verify email or user</response> 
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword([Required] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var forgotPasswordLink = Url.Action(nameof(ResetPassword), "Auth", new { token, email = user.Email }, Request.Scheme);
                // var message = new Message(new string[] { user.Email! }, "Forgot password link", $"Click on the link {forgotPasswordLink!} to reset your password ");
                var messageBody = EmailTemplateProvider.ResetPassword($"{user.FirstName} {user.LastName}", forgotPasswordLink);
                var message = new Message(new string[] { user.Email! }, "Reset Password", messageBody);
                _emailService.SendEmail(message);
                return StatusCode(StatusCodes.Status200OK,
                    new APIResponse { StatusCode = "Success", IsSuccess = true, Message = $"Password reset link sent to {user.Email} successfully" });
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                        new APIResponse { StatusCode = "Error", IsSuccess = false, Message = "Reset password fail! Please verify your email and try again." });
        }


        /// <summary>
        ///  Reset/Change Password
        /// </summary>
        /// <param name="token"></param>
        /// <param name="email"></param>
        /// <response code="400">Validation Error</response> 
        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var passwordReset = new PasswordReset { Token = token, Email = email };

            return Ok(new
            {
                passwordReset
            });
        }

        /// <summary>
        ///  Change to a new password
        /// </summary>
        /// <param name="resetpassword"></param>
        /// <response code="200">Password changed successfully!</response> 
        /// <response code="400">Reset password failed</response> 
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


        /// <summary>
        /// Login Using the two factor auth token
        /// </summary>
        /// <param name="otpCode"></param>
        /// <param name="email"></param>
        /// <response code="200">Login Successfull</response> 
        /// <response code="400">Invalid Token/Validation error</response> 
        [HttpPost("Login-2FA")]
        public async Task<IActionResult> LoginWithOTP(string otpCode, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var signIn = await _signInManager.TwoFactorSignInAsync("Email", otpCode, false, false);
            if (signIn.Succeeded)
            {
                if (user != null)
                {
                    var authClaims = new List<Claim>
                    {
                     new Claim(ClaimTypes.Name, user.UserName),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };
                    var userRoles = await _userManager.GetRolesAsync(user);
                    foreach (var role in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, role));
                    }
                    var JwtToken = GetToken(authClaims);
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(JwtToken),
                        expiration = JwtToken.ValidTo
                    });
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                        new APIResponse { StatusCode = "Error", IsSuccess = false, Message = "Invalid token" });
        }
    }
}
