using HRPortalApi.Interface;
using HRPortalApi.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HRPortalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CompanyRegister data)
        {
            var result = await _companyService.Register(data);
            return result;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _companyService.Login(request.Email, request.Password);
            return result;
        }

        [HttpPost("vacancy")]
        public async Task<IActionResult> CreateVacancy([FromBody] NewVacancy data)
        {
            var result = await _companyService.CreateVacancy(data);
            return result;
        }

        [HttpPut("vacancy")]
        public async Task<IActionResult> UpdateVacancy([FromBody] NewVacancy data)
        {
            var result = await _companyService.UpdateVacancy(data);
            return result;
        }

        [HttpDelete("vacancy/{id}")]
        public async Task<IActionResult> DeleteVacancy(int id)
        {
            var result = await _companyService.DeleteVacancy(id);
            return result;
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] CompanyRegister data)
        {
            var result = await _companyService.UpdateProfile(data);
            return result;
        }
    }

    // DTO для входа
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}