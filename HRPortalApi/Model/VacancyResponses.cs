using HRPortalApi.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class VacancyResponses
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    public int VacancyId { get; set; }

    public DateTime ResponseDate { get; set; }

    public string Status { get; set; } // Pending, Accepted, Rejected

    [ForeignKey("StudentId")]
    public Student Student { get; set; }

    [ForeignKey("VacancyId")]
    public Vacancy Vacancy { get; set; }
}