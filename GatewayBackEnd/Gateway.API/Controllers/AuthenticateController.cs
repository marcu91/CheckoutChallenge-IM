using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Gateway.API.Authentication;
using Gateway.Shared.Representers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Gateway.API.Controllers
{
    /// <summary>
    /// A controller responsible for login and registration of gateway merchants and gateway admins
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// A constructor for the authentication controller of type <see cref="AuthenticateController"/>
        /// </summary>
        /// <param name="userManager">The user manager used to create and check user credentials</param>
        /// <param name="roleManager">The role manager of type create and asign roles </param>
        /// <param name="configuration">The configuration object for retrieving config data</param>
        public AuthenticateController(
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        /// <summary>
        /// A controller action that is used to login a user
        /// </summary>
        /// <param name="model">A login model that is retrieved from the body of the request</param>
        /// <returns>An action result containing the token data</returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("Login failed using model: {model}", model);
                return BadRequest();
            }

            var user = await GetAppUser(model.Username).ConfigureAwait(false);
            if (user == null)
            {
                Log.Warning("Login failed. Unauthorized user: {username}", model.Username);
                return Unauthorized();
            }

            var passwordIsValid = await userManager.CheckPasswordAsync(user, model.Password).ConfigureAwait(false);
            if (!passwordIsValid)
            {
                Log.Warning("Login failed. Incorrect password for user: {username}", model.Username);
                return Unauthorized();
            }

            JwtSecurityToken token = await CreateJwtToken(user).ConfigureAwait(false);

            var response = new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            };

            return Ok(response);
        }

        /// <summary>
        /// A controller action that is used to register a new gateway merchant user 
        /// </summary>
        /// <param name="model">The registration model</param>
        /// <returns>A response containing the new user</returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            return await CreateUserWithRole(model, UserRoles.GatewayMerchantUser).ConfigureAwait(false);
        }

        /// <summary>
        /// A controller action that is used to register a new gateway admin user 
        /// </summary>
        /// <param name="model">The registration model</param>
        /// <returns>A response containing the new user</returns>
        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            return await CreateUserWithRole(model, UserRoles.GatewayAdministrator).ConfigureAwait(false);
        }

        #region Non-Action methods 
        [NonAction]
        public async Task<IActionResult> CreateUserWithRole(RegisterModel model, string userRole)
        {
            if (model == null)
            {
                Log.Warning("User creation failed. No register model");
                return BadRequest();
            }

            var userExists = await this.GetAppUser(model.Username).ConfigureAwait(false);
            if (userExists != null)
            {
                Log.Warning("User creation failed. Duplicate found.");
                var errorMessage = "User creation failed! Please check user details and try again.";
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = errorMessage });
            }

            var user = new ApplicationUser(model);
            var result = await userManager.CreateAsync(user, model.Password).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                Log.Warning("User creation failed. Errors : {errors}", result.Errors);
                var errorList = result.Errors;
                return BadRequest(errorList);
            }

            await CreateAuthorizationRoles().ConfigureAwait(false);

            if (await roleManager.RoleExistsAsync(userRole).ConfigureAwait(false))
            {
                await userManager.AddToRoleAsync(user, userRole).ConfigureAwait(false);
            }

            JwtSecurityToken token = await CreateJwtToken(user).ConfigureAwait(false);

            var response = new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            };

            return Ok(response);
        }

        [NonAction]
        public async Task<ApplicationUser> GetAppUser(string userName)
        {
            return await userManager.FindByNameAsync(userName).ConfigureAwait(false);
        }

        [NonAction]
        public async Task CreateAuthorizationRoles()
        {
            //Create gateway admin role, if not existent
            if (!await roleManager.RoleExistsAsync(UserRoles.GatewayAdministrator).ConfigureAwait(false))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.GatewayAdministrator)).ConfigureAwait(false);

            //Create gateway merchant role, if not existent
            if (!await roleManager.RoleExistsAsync(UserRoles.GatewayMerchantUser).ConfigureAwait(false))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.GatewayMerchantUser)).ConfigureAwait(false);
        }

        [NonAction]
        public async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userRoles = await userManager.GetRolesAsync(user).ConfigureAwait(false);
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }

        #endregion
    }
}