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
    public class StudentService : IStudentService
    {
        private readonly ContextDb _context;

        public StudentService(ContextDb context)
        {
            _context = context;
        }

        public async Task<IActionResult> Register(StudentRegister data)
        {
            try
            {
                // Проверяем, существует ли пользователь с таким email
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == data.email);

                if (existingUser != null)
                {
                    return new BadRequestObjectResult(new { message = "Пользователь с таким email уже существует" });
                }

                // Создаем нового пользователя
                var user = new User
                {
                    Username = data.email,
                    Password = data.password // В реальном приложении пароль должен хэшироваться!
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                // Создаем студента
                var student = new Student
                {
                    Surname = data.Surname,
                    Name = data.Name,
                    Patronymic = data.Patronymic,
                    Birthday = data.Birthday,
                    Institution = data.institution,
                    Speciality = data.Speciality,
                    Grade = data.Grade,
                    UserId = user.Id
                };

                await _context.Students.AddAsync(student);
                await _context.SaveChangesAsync();

                return new OkObjectResult(new
                {
                    message = "Студент успешно зарегистрирован",
                    studentId = student.Id,
                    userId = user.Id
                });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { message = $"Ошибка при регистрации студента: {ex.Message}" });
            }
        }

        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == email && u.Password == password);

                if (user == null)
                {
                    return new UnauthorizedObjectResult(new { message = "Неверный email или пароль" });
                }

                // Получаем студента
                var student = await _context.Students
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.UserId == user.Id);

                if (student == null)
                {
                    return new NotFoundObjectResult(new { message = "Студент не найден для данного пользователя" });
                }

                return new OkObjectResult(new
                {
                    message = "Успешный вход",
                    userId = user.Id,
                    studentId = student.Id,
                    fullName = $"{student.Surname} {student.Name} {student.Patronymic}"
                });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { message = $"Ошибка при входе: {ex.Message}" });
            }
        }

        public async Task<IActionResult> UpdateProfile(StudentRegister data, int id)
        {
            try
            {
                var student = await _context.Students
                    .Where(s => s.Id == id)
                    .FirstOrDefaultAsync();

                if (student == null)
                {
                    return new NotFoundObjectResult(new { message = "Студент не найден" });
                }

                // Обновляем данные студента
                student.Surname = data.Surname;
                student.Name = data.Name;
                student.Patronymic = data.Patronymic;
                student.Birthday = data.Birthday;
                student.Institution = data.institution;
                student.Speciality = data.Speciality;
                student.Grade = data.Grade;

                // Обновляем данные пользователя (email)
                if (student.User != null)
                {
                    student.User.Username = data.email;
                    // Пароль обновляется только если предоставлен новый
                    if (!string.IsNullOrEmpty(data.password))
                    {
                        student.User.Password = data.password; // В реальном приложении - хэширование!
                    }
                }

                _context.Students.Update(student);
                await _context.SaveChangesAsync();

                return new OkObjectResult(new { message = "Профиль успешно обновлен", studentId = student.Id });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { message = $"Ошибка при обновлении профиля: {ex.Message}" });
            }
        }

        public async Task<IActionResult> VacancyResponse(int StudentId, int VacancyId)
        {
            try
            {
                // Проверяем существование студента
                var student = await _context.Students.FindAsync(StudentId);
                if (student == null)
                {
                    return new NotFoundObjectResult(new { message = "Студент не найден" });
                }

                // Проверяем существование вакансии
                var vacancy = await _context.Vacancies.FindAsync(VacancyId);
                if (vacancy == null)
                {
                    return new NotFoundObjectResult(new { message = "Вакансия не найдена" });
                }

                // Проверяем, не откликался ли уже студент на эту вакансию
                var existingResponse = await _context.VacancyResponses
                    .FirstOrDefaultAsync(vr => vr.StudentId == StudentId && vr.VacancyId == VacancyId);

                if (existingResponse != null)
                {
                    return new BadRequestObjectResult(new { message = "Вы уже откликались на эту вакансию" });
                }

                // Создаем отклик на вакансию
                var vacancyResponse = new VacancyResponses
                {
                    StudentId = StudentId,
                    VacancyId = VacancyId,
                    ResponseDate = DateTime.UtcNow,
                    Status = "Pending" // На рассмотрении
                };

                await _context.VacancyResponses.AddAsync(vacancyResponse);
                await _context.SaveChangesAsync();

                return new OkObjectResult(new
                {
                    message = "Отклик на вакансию успешно отправлен",
                    responseId = vacancyResponse.Id
                });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { message = $"Ошибка при отправке отклика: {ex.Message}" });
            }
        }
    }
}