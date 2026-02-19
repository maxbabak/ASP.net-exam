namespace RecruitmentApi1._0.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentApi1._0.Data;
using RecruitmentApi1._0.DTOs.Applications;
using RecruitmentApi1._0.Enums;
using RecruitmentApi1._0.Models;
using System.Security.Claims;

[ApiController]
[Route("applications")]
[Authorize]
public class ApplicationsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ApplicationsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("user")]
    public async Task<IActionResult> MyApplications()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var apps = await _db.Applications
            .Where(x => x.UserId == userId)
            .Include(x => x.JobPost)
            .ToListAsync();

        return Ok(apps);
    }

    [Authorize(Roles = "User")]
    [HttpPost("{jobId}")]
    public async Task<IActionResult> Apply(int jobId, ApplyDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var app = new Application
        {
            JobPostId = jobId,
            UserId = userId,
            AppliedDate = DateTime.UtcNow,
            CoverLetter = dto.CoverLetter,
            Status = ApplicationStatus.Pending
        };

        _db.Applications.Add(app);
        await _db.SaveChangesAsync();

        return Ok(app);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, UpdateStatusDto dto)
    {
        var app = await _db.Applications.FindAsync(id);
        if (app == null) return NotFound();

        app.Status = dto.Status;
        await _db.SaveChangesAsync();

        return Ok(app);
    }
}