using HRPortalApi.Interface;
using HRPortalApi.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HRPortalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] StudentRegister data)
        {
            var result = await _studentService.Register(data);
            return result;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _studentService.Login(request.Email, request.Password);
            return result;
        }

        [HttpPut("profile/{id}")]
        public async Task<IActionResult> UpdateProfile([FromBody] StudentRegister data, int id)
        {
            var result = await _studentService.UpdateProfile(data, id);
            return result;
        }

        [HttpPost("vacancy-response")]
        public async Task<IActionResult> VacancyResponse([FromQuery] int studentId, [FromQuery] int vacancyId)
        {
            var result = await _studentService.VacancyResponse(studentId, vacancyId);
            return result;
        }
    }
}