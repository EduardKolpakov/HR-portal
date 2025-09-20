using HRPortalApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HRPortalApi.Interface
{
    public interface ICompanyService
    {
        public Task<IActionResult> Register(CompanyRegister data);
        public Task<IActionResult> Login(string email, string password);
        public Task<IActionResult> CreateVacancy(NewVacancy data);
        public Task<IActionResult> UpdateVacancy(NewVacancy data);
        public Task<IActionResult> DeleteVacancy(int VacancyId);
        public Task<IActionResult> UpdateProfile(CompanyRegister data);
    }
}
