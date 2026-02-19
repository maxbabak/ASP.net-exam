namespace RecruitmentApi1._0.DTOs.Applications;

using System.ComponentModel.DataAnnotations;

public class ApplyDto
{
    [Required]
    public string CoverLetter { get; set; } = null!;
}