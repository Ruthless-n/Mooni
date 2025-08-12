using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BCrypt.Net;
using backend.Data;
using backend.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly _dbContext? _context;

    public AuthController(_dbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult> GetUsers()
    {
        if (_context == null)
        {
            return NotFound();
        }
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }

    [HttpPost]
    [Route("create")]
    public async Task<ActionResult> CreateUser([FromQuery] UserCreateDTO dto)
    {
        if (_context == null)
            return NotFound();

        if (!IsUserDataValid(dto, out var validationError))
            return BadRequest(validationError);

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (existingUser != null)
            return BadRequest("User already exists.");

        var user = MapToUser(dto);
        NormalizeBirthDate(user);

        if (!IsBirthDateValid(user.BirthDate, out var birthDateError))
            return BadRequest(birthDateError);

        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUsers), user);
    }

    // Valida dados obrigatórios do DTO
    private bool IsUserDataValid(UserCreateDTO dto, out string? error)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) ||
            string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrWhiteSpace(dto.Password))
        {
            error = "User data is incomplete.";
            return false;
        }
        error = null;
        return true;
    }

    private Users MapToUser(UserCreateDTO dto) => new Users
    {
        Name = dto.Name,
        Email = dto.Email,
        Password = dto.Password,
        BirthDate = dto.BirthDate
    };

    private void NormalizeBirthDate(Users user)
    {
        if (user.BirthDate.Kind == DateTimeKind.Unspecified)
            user.BirthDate = DateTime.SpecifyKind(user.BirthDate, DateTimeKind.Utc);
    }

    private bool IsBirthDateValid(DateTime birthDate, out string? error)
    {
        var now = DateTime.UtcNow;
        if (birthDate > now)
        {
            error = "Birth date cannot be in the future.";
            return false;
        }
        if (birthDate < now.AddYears(-120))
        {
            error = "Birth date is too far in the past.";
            return false;
        }
        error = null;
        return true;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> LoginUser([FromQuery] LoginRequest checkUser)
    {
        if (_context == null)
        {
            return NotFound();
        }
        if (checkUser.Email == null || checkUser.Password == null)
        {
            return BadRequest("Email and password are required.");
        }

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == checkUser.Email);
        if (existingUser == null || !BCrypt.Net.BCrypt.Verify(checkUser.Password, existingUser.Password))
        {
            return Unauthorized("Invalid email or password.");
        }

        return Ok(existingUser);
    }

    [HttpGet]
    [Route("password/get-token")]
    public async Task<ActionResult> GetPasswordResetToken([FromQuery] string email)
    {
        if (_context == null)
        {
            return NotFound();
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            return BadRequest("Invalid email.");
        }

        // Generate a password reset token (this is just a placeholder, implement your own logic)
        var token = Guid.NewGuid().ToString();

        // Here you would typically send the token to the user's email
        return Ok(new { Token = token });
    }
}
