namespace RecruitmentApi1._0.Services.Interfaces;

using RecruitmentApi1._0.Models;

public interface IJwtService
{
    string GenerateToken(User user);
}