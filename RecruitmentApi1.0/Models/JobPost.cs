namespace RecruitmentApi1._0.Models;

public class JobPost
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Requirements { get; set; } = null!;
    public DateTime PostedDate { get; set; }
    public bool IsActive { get; set; }

    public ICollection<Application> Applications { get; set; } = new List<Application>();
}