using Microsoft.AspNetCore.Mvc;
using HRPortalApi.Requests;

namespace HRPortalApi.Interface
{
    public interface INotificationService
    {
        public Task<IActionResult> Create(NewNotification data, int UserId);
        public Task<IActionResult> Read(int id);
    }
}
