using Microsoft.EntityFrameworkCore;
using RecruitmentApi1._0.Models;

namespace RecruitmentApi1._0.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
}