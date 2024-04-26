using Domain.StudentCRUD;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.StudentCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

          
            var user = new AppUser { UserName = model.Email, Email = model.Email };

            // Create user
            await _userManager.CreateAsync(user, "Password123!");

            // Assign role to user
            await _userManager.AddToRoleAsync(user, "User");

            // Check if the specified role exists
            var roleExists = await _roleManager.RoleExistsAsync(model.Role!);
            if (!roleExists)
            {
                // If the role doesn't exist, return error
                return BadRequest("Invalid role specified.");
            }

            var result = await _userManager.CreateAsync(user, model.Password!);
            if (result.Succeeded)
            {
                // Assign the specified role to the user
                await _userManager.AddToRoleAsync(user, model.Role!);



                return Ok("User registered successfully.");
            }

            return BadRequest(result.Errors);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUsers(string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok("User deleted successfully.");
            }
            return BadRequest(result.Errors);
        }


        [HttpGet("ByEmail")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound(); // Return 404 Not Found if user with given email is not found
            }

            return Ok(user);
        }


        [HttpPut("ByEmail")]
            public async Task<IActionResult> Update(string email, UpdateViewModel model)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userManager.FindByEmailAsync(model.Email!);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                user.Email = model.Email;
                user.UserName = model.Email;

                // Check if the specified role exists
                var roleExists = await _roleManager.RoleExistsAsync(model.Role!);
                if (!roleExists)
                {
                    // If the role doesn't exist, return error
                    return BadRequest("Invalid role specified.");
                }

                // Get the roles assigned to the user
                var roles = await _userManager.GetRolesAsync(user);

                // Remove existing roles
                await _userManager.RemoveFromRolesAsync(user, roles.ToArray());

                // Assign the specified role to the user
                await _userManager.AddToRoleAsync(user, model.Role!);

                // Check if password change is requested
                if (!string.IsNullOrEmpty(model.Password))
                {
                    var passwordChangeResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword!, model.Password);
                    if (!passwordChangeResult.Succeeded)
                    {
                        return BadRequest(passwordChangeResult.Errors);
                    }
                }

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok("User email, role, and password updated successfully.");
                }

                return BadRequest(result.Errors);
            }
        }

    }
   
