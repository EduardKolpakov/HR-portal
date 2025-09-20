using HRPortalApi.Interface;
using HRPortalApi.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HRPortalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NewNotification data, [FromQuery] int userId)
        {
            var result = await _notificationService.Create(data, userId);
            return result;
        }

        [HttpPut("read/{id}")]
        public async Task<IActionResult> Read(int id)
        {
            var result = await _notificationService.Read(id);
            return result;
        }
    }
}