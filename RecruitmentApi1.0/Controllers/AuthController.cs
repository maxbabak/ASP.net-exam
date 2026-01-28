using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentApi1._0.Data;
using RecruitmentApi1._0.DTOs;
using RecruitmentApi1._0.Models;
using RecruitmentApi1._0.Services;

namespace RecruitmentApi1._0.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtTokenService _jwtService;

    public AuthController(AppDbContext context, JwtTokenService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            return BadRequest("Email already exists");

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = PasswordHasher.Hash(dto.Password),
            Role = "User"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Registered successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var passwordHash = PasswordHasher.Hash(dto.Password);

        var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.Email == dto.Email && u.PasswordHash == passwordHash);

        if (user == null)
            return Unauthorized("Invalid credentials");

        var token = _jwtService.GenerateToken(user);
        return Ok(new { token });
    }
}