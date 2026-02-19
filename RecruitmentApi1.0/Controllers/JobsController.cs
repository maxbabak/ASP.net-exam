namespace RecruitmentApi1._0.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentApi1._0.Data;
using RecruitmentApi1._0.DTOs.Jobs;
using RecruitmentApi1._0.Models;

[ApiController]
[Route("jobs")]
public class JobsController : ControllerBase
{
    private readonly AppDbContext _db;

    public JobsController(AppDbContext db)
    {
        _db = db;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateJobDto dto)
    {
        var job = new JobPost
        {
            Title = dto.Title,
            Description = dto.Description,
            Requirements = dto.Requirements,
            PostedDate = DateTime.UtcNow,
            IsActive = true
        };

        _db.JobPosts.Add(job);
        await _db.SaveChangesAsync();

        return Ok(job);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _db.JobPosts.Where(x => x.IsActive).ToListAsync());
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var job = await _db.JobPosts.FindAsync(id);
        if (job == null) return NotFound();

        _db.JobPosts.Remove(job);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}