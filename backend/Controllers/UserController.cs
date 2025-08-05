using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly _dbContext? _context;

    public UserController(_dbContext context)
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
    public async Task<ActionResult> CreateUser([FromQuery] Users user)
    {
        if (_context == null)
        {
            return NotFound();
        }

        if (user.Email == null || user.Password == null || user.Name == null)
        {
            return BadRequest("User data is incomplete.");
        }

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        if (existingUser != null)
        {
            return BadRequest("User already exists.");
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUsers), user);
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
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == checkUser.Email && u.Password == checkUser.Password);
        if (existingUser == null)
        {
            return Unauthorized("Invalid email or password.");
        }
        return Ok(existingUser);
    }
}
