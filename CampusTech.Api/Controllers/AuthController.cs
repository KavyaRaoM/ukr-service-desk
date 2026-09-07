using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager= roleManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        User user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            UserName = dto.Email
        };

        IdentityResult result =
            await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        if (!await _roleManager.RoleExistsAsync("User")) //Does a role called "User" already exist?
        {
            await _roleManager.CreateAsync(new IdentityRole<int>("User")//Create a role named "User"
            );
        }

    await _userManager.AddToRoleAsync(user, "User");//Take the user we just registered and assign them to the "User" role.

        return Ok(new
        {
            user.Id,
            user.Name,
            user.Email
        });
    }

    [HttpPost("register-admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegisterAdmin(RegisterDto dto)
    {
        User user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            UserName = dto.Email
        };

        IdentityResult result =
            await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        if (!await _roleManager.RoleExistsAsync("Admin"))
        {
            await _roleManager.CreateAsync(
                new IdentityRole<int>("Admin")
            );
        }

        await _userManager.AddToRoleAsync(user, "Admin");

        return Ok(new
        {
            user.Id,
            user.Name,
            user.Email,
            Role = "Admin"
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        User? user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
        {
            return Unauthorized("Invalid email or password.");
        }

        bool passwordValid =
            await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!passwordValid)
        {
            return Unauthorized("Invalid email or password.");
        }

        IList<string> roles =
            await _userManager.GetRolesAsync(user);

        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.Name)
        };

        foreach (string role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        SymmetricSecurityKey key =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!
                )
            );

        SigningCredentials credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

        JwtSecurityToken token =
            new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

        string tokenString =
            new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new
        {
            token = tokenString,
            user = new
            {
                user.Id,
                user.Name,
                user.Email,
                roles
            }
        });
    }
}