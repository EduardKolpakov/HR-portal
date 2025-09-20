namespace HRPortalApi.Requests
{
    public class NewVacancy
    {
        public string Position { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int Experience { get; set; }
        public int SalaryMin { get; set; }
        public int SalaryMax { get; set; }
        public string Skills { get; set; }

    }
}
