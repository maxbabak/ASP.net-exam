namespace RecruitmentApi1._0.Models;

using RecruitmentApi1._0.Enums;

public class Application
{
    public int Id { get; set; }

    public int JobPostId { get; set; }
    public JobPost JobPost { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public DateTime AppliedDate { get; set; }
    public string CoverLetter { get; set; } = null!;
    public ApplicationStatus Status { get; set; }
}