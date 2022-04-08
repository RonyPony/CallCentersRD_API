using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using CallCentersRD_API.Controllers.Base.Auth;
using CallCentersRD_API.Controllers.Base.Auth.DTOs;
using CallCentersRD_API.Database.DTOs.Auth;
using CallCentersRD_API.Database.Entities.Auth;
using CallCentersRD_API.Database.Services.Constructor;
using CallCentersRD_API.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CallCentersRD_API.Controllers.Auth;

[AllowAnonymous]
//[Authorize(Roles = Roles.Admin)]
public class UserAuthController : BaseAuthorizationController<User, Guid>
{
    public UserAuthController(IServiceConstructor _constructor, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, IConfiguration configuration) : base(_constructor, userManager, signInManager, roleManager, configuration)
    {
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<Result<Object>>> Register([FromBody] RegisterUser userModel)
    {
        try
        {
            User userExists;

            var userExistsByName = await _userManager.FindByNameAsync(userModel.UserName);

            var userExistsByEmail = await _userManager.FindByEmailAsync(userModel.Email);

            if (userExistsByName != null || userExistsByEmail != null)
                return StatusCode(StatusCodes.Status500InternalServerError, Result.Instance().Fail("User already exists!"));

            var user = new User()
            {
                Name = userModel.Name,
                LastName = userModel.LastName,
                Email = userModel.Email,
                UserName = userModel.UserName,
                RegistrationDate = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(Roles.User))
                    await _roleManager.CreateAsync(new Role(Roles.User));
                if (await _roleManager.RoleExistsAsync(Roles.User))
                {
                    await _userManager.AddToRoleAsync(user, Roles.User);
                }
                return Ok(Result.Instance().Success($"User '{userModel.UserName} <{userModel.Email}>' created !"));
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden, Result.Instance().Fail($"User not created. Check the provided information and try again !.", data: result.Errors));
            }
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, Result.Instance().Fail("There an internal error in the server, User not created. Check the provided information and try again !", exception: e));
        }
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("register/admin")]
    public async Task<ActionResult<Result<Object>>> RegisterAdmin([FromBody] RegisterUser userModel)
    {
        try
        {
            User userExists;

            var userExistsByName = await _userManager.FindByNameAsync(userModel.UserName);

            var userExistsByEmail = await _userManager.FindByEmailAsync(userModel.Email);

            if (userExistsByName != null || userExistsByEmail != null)
                return StatusCode(StatusCodes.Status500InternalServerError, Result.Instance().Fail("User already exists!"));

            var user = new User()
            {
                Name = userModel.Name,
                LastName = userModel.LastName,
                Email = userModel.Email,
                UserName = userModel.UserName,
                RegistrationDate = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            if (result.Succeeded)
            {

                if (!await _roleManager.RoleExistsAsync(Roles.Admin))
                    await _roleManager.CreateAsync(new Role(Roles.Admin));

                if (await _roleManager.RoleExistsAsync(Roles.Admin))
                {
                    await _userManager.AddToRoleAsync(user, Roles.Admin);
                }

                return Ok(Result.Instance().Success($"User '{userModel.UserName} <{userModel.Email}>' created !"));
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden, Result.Instance().Fail("User not created. Check the provided information and try again !", data: result.Errors));
            }
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, Result.Instance().Fail("There an internal error in the server, User not created. Check the provided information and try again !", exception: e));
        }
    }

    [HttpGet]
    [AllowAnonymous]
    //[Authorize(Roles = $"{Roles.Admin},{Roles.User}")]
    [Route("user/{email}")]
    public async Task<ActionResult<Result<User>>> GetUser([FromRoute(Name = "email")] string email)
    {
        try
        {
            var userFinded = await _userManager.FindByEmailAsync(email);

            if (userFinded != null)
            {
                return Result.Instance<User>().Success("user fined", userFinded);
            }
            else
            {
                return Result.Instance<User>().Fail($"User not finded. Please check your email");
            }
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, Result.Instance<Object>().Fail("Unable to get the user", exception: e));
        }
    }

    [HttpDelete]
    [Authorize(Roles = $"{Roles.Admin},{Roles.User}")]
    [Route("remove")]
    public async Task<ActionResult<Result<Object>>> DeleteUser()
    {
        try
        {
            var userFinded = await _userManager.FindByEmailAsync(this.User.FindFirst(c => c.Type == ClaimTypes.Email).Value);

            var result = await _userManager.DeleteAsync(userFinded);

            if (result.Succeeded)
            {
                return Result.Instance().Success($"User <{userFinded.Email}> deleted !");
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, Result.Instance().Fail("Theres an error when trying to delete the user", data: result.Errors));
            }
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, Result.Instance().Fail("Unable to delete the user", exception: e));
        }
    }

    [HttpDelete]
    [Authorize(Roles = Roles.Admin)]
    [Route("remove/{userId}")]
    public async Task<ActionResult<Result<Object>>> DeleteByID([FromRoute] string userId)
    {
        try
        {
            var userFinded = await _userManager.FindByIdAsync(userId);

            if (userFinded != null)
            {
                var result = await _userManager.DeleteAsync(userFinded);

                if (result.Succeeded)
                {
                    return Result.Instance().Success($"User <{userFinded.Email}> deleted !");
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, Result.Instance().Fail("Theres an error when trying to delete the user", data: result.Errors));
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, Result.Instance().Fail("The requested user was't found !"));
            }
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, Result.Instance().Fail("Unable to delete the user", exception: e));
        }
    }

    protected override async Task<(Result<Object> result, User? entity, string jwt, DateTime expiration)> AuthorizeAccess(Credentials credentials)
    {
        try
        {
            User userFinded;

            if (new EmailAddressAttribute().IsValid(credentials.UserName))
            {
                userFinded = await _userManager.FindByEmailAsync(credentials.UserName);
            }
            else
            {
                userFinded = await _userManager.FindByNameAsync(credentials.UserName);
            }

            var loginResult = await _signInManager.PasswordSignInAsync(userFinded, credentials.Password, credentials.RememberMe, false);

            if (loginResult.Succeeded)
            {

                var userRoles = await _userManager.GetRolesAsync(userFinded);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userFinded.UserName),
                    new Claim(ClaimTypes.Email, userFinded.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                return (result: Result.Instance().Success("Login successful !"), entity: userFinded, jwt: new JwtSecurityTokenHandler().WriteToken(token), expiration: token.ValidTo);
            }
            return (result: Result.Instance().Fail("Login failed !"), entity: null, jwt: String.Empty, expiration: DateTime.Now);
        }
        catch (Exception e)
        {
            return (Result.Instance().Fail("There is an error on the authorize validation", e), null, String.Empty, DateTime.Now);
        }
    }

    protected override Task<(Result<Object> result, User? entity)> ChangePassword(string email, string newPassword, string actualPassword = null, string resetToken = null)
    {
        throw new NotImplementedException();
    }

    protected override Task<Claim> GetRoleClaims(User entity)
    {
        throw new NotImplementedException();
    }

    protected override async Task<Result<Object>> LogOut(string token)
    {
        await _signInManager.SignOutAsync();
        return Result.Instance().Success("User Logout !");
    }

    protected override Task<(Result<Object> result, User? entity, string confirmationToken)> RequestEmailValidation(string email)
    {
        throw new NotImplementedException();
    }

    protected override Task<(Result<Object> result, User? entity, string requestToken)> RequestPasswordRecovery(string username, IPAddress ip = null)
    {
        throw new NotImplementedException();
    }

    protected override Task<(Result<Object> result, User? entity)> ValidateEmail(string email, string confirmationToken)
    {
        throw new NotImplementedException();
    }
}