using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRPortalApi.Model
{
    public class Student
    {
        [Key] 
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public DateOnly Birthday { get; set; }
        public string institution { get; set; } //Учебное заведение
        public string Speciality { get; set; }
        public int Grade { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
