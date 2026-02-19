namespace RecruitmentApi1._0.Models;

using RecruitmentApi1._0.Enums;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public Role Role { get; set; }

    public ICollection<Application> Applications { get; set; } = new List<Application>();
}