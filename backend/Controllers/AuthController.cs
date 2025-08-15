using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
            return BadRequest("Usuário já existe em nosso sistema.");

        var user = MapToUser(dto);
        NormalizeBirthDate(user);

        if (!IsBirthDateValid(user.BirthDate, out var birthDateError))
            return BadRequest(birthDateError);

        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUsers), user);
    }

    private bool IsUserDataValid(UserCreateDTO dto, out string? error)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) ||
            string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrWhiteSpace(dto.Password))
        {
            error = "Dados do usuário incompletos.";
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
            error = "Data de nascimento inválida.";
            return false;
        }
        if (birthDate < now.AddYears(-120))
        {
            error = "Data de nascimento muito antiga.";
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
            return BadRequest("Por favor, insira seu email e senha.");
        }

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == checkUser.Email);
        if (existingUser == null || !BCrypt.Net.BCrypt.Verify(checkUser.Password, existingUser.Password))
        {
            return Unauthorized("Email ou senha inválidos.");
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
            return BadRequest("Email inválido.");
        }

        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(Config.LoadJwtExpiration()))
        );

        return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}
