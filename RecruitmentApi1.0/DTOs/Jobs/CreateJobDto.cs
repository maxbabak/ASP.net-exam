namespace RecruitmentApi1._0.DTOs.Jobs;

using System.ComponentModel.DataAnnotations;

public class CreateJobDto
{
    [Required]
    public string Title { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;

    [Required]
    public string Requirements { get; set; } = null!;
}