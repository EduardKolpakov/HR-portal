namespace HRPortalApi.Requests
{
    public class StudentRegister
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public DateOnly Birthday { get; set; }
        public string institution { get; set; } //Учебное заведение
        public string Speciality { get; set; }
        public int Grade { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}
