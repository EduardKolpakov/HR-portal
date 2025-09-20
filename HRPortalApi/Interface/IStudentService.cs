using HRPortalApi.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HRPortalApi.Interface
{
    public interface IStudentService
    {
        public Task<IActionResult> Register(StudentRegister data);
        public Task<IActionResult> Login(string email, string password);
        public Task<IActionResult> UpdateProfile(StudentRegister data, int id);
        public Task<IActionResult> VacancyResponse(int StudentId, int VacancyId);
    }
}