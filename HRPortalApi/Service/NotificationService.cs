using HRPortalApi.DBModel;
using HRPortalApi.Interface;
using HRPortalApi.Model;
using HRPortalApi.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HRPortalApi.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ContextDb _context;

        public NotificationService(ContextDb context)
        {
            _context = context;
        }

        public async Task<IActionResult> Create(NewNotification data, int UserId)
        {
            try
            {
                // Проверяем существование пользователя
                var user = await _context.Users.FindAsync(UserId);
                if (user == null)
                {
                    return new NotFoundObjectResult(new { message = "Пользователь не найден" });
                }

                var notification = new Notifications
                {
                    Title = data.Title,
                    Content = data.Content,
                    Type = data.Type,
                    IsRead = data.IsRead,
                    UserId = UserId
                    // Timestamp автоматически устанавливается в модели
                };

                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();

                return new OkObjectResult(new
                {
                    message = "Уведомление успешно создано",
                    notificationId = notification.Id
                });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { message = $"Ошибка при создании уведомления: {ex.Message}" });
            }
        }

        public async Task<IActionResult> Read(int id)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(id);
                if (notification == null)
                {
                    return new NotFoundObjectResult(new { message = "Уведомление не найдено" });
                }

                // Помечаем уведомление как прочитанное
                notification.IsRead = true;
                _context.Notifications.Update(notification);
                await _context.SaveChangesAsync();

                return new OkObjectResult(new
                {
                    message = "Уведомление помечено как прочитанное",
                    notificationId = notification.Id
                });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { message = $"Ошибка при обновлении уведомления: {ex.Message}" });
            }
        }
    }
}