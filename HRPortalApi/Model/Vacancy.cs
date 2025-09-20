using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRPortalApi.Model
{
    public class Vacancy
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int Experience { get; set; }
        public int SalaryMin { get; set; }
        public int SalaryMax { get; set; }
        public string Skills { get; set; }
    }
}
