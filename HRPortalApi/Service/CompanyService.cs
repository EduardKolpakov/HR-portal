using HRPortalApi.Interface;
using HRPortalApi.Model;
using HRPortalApi.DBModel;
using HRPortalApi.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HRPortalApi.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ContextDb _context;

        public CompanyService(ContextDb context)
        {
            _context = context;
        }

        public async Task<IActionResult> Register(CompanyRegister data)
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

                // Создаем компанию
                var company = new Company
                {
                    Name = data.Name,
                    Description = data.Description,
                    Location = data.Location,
                    Category = data.Category,
                    UserId = user.Id
                };

                await _context.Companies.AddAsync(company);
                await _context.SaveChangesAsync();

                return new OkObjectResult(new
                {
                    message = "Компания успешно зарегистрирована",
                    companyId = company.Id,
                    userId = user.Id
                });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { message = $"Ошибка при регистрации компании: {ex.Message}" });
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

                // Получаем компанию пользователя
                var company = await _context.Companies
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.UserId == user.Id);

                if (company == null)
                {
                    return new NotFoundObjectResult(new { message = "Компания не найдена для данного пользователя" });
                }

                // В реальном приложении здесь генерируется JWT токен
                return new OkObjectResult(new
                {
                    message = "Успешный вход",
                    userId = user.Id,
                    companyId = company.Id,
                    companyName = company.Name,
                    email = company.User.Username
                });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { message = $"Ошибка при входе: {ex.Message}" });
            }
        }

        public async Task<IActionResult> CreateVacancy(NewVacancy data)
        {
            try
            {
                // В реальном приложении companyId должен получаться из контекста (токена)
                // Для примера берем первую компанию
                var company = await _context.Companies.FirstOrDefaultAsync();
                if (company == null)
                {
                    return new BadRequestObjectResult(new { message = "Компания не найдена" });
                }

                var vacancy = new Vacancy
                {
                    CompanyId = company.Id,
                    Position = data.Position,
                    Description = data.Description,
                    Location = data.Location,
                    Experience = data.Experience,
                    SalaryMin = data.SalaryMin,
                    SalaryMax = data.SalaryMax,
                    Skills = data.Skills
                };

                await _context.Vacancies.AddAsync(vacancy);
                await _context.SaveChangesAsync();

                return new OkObjectResult(new { message = "Вакансия успешно создана", vacancyId = vacancy.Id });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { message = $"Ошибка при создании вакансии: {ex.Message}" });
            }
        }

        public async Task<IActionResult> UpdateVacancy(NewVacancy data)
        {
            try
            {
                // В реальном приложении vacancyId должен передаваться как параметр
                // Для примера берем первую вакансию
                var vacancy = await _context.Vacancies.FirstOrDefaultAsync();
                if (vacancy == null)
                {
                    return new NotFoundObjectResult(new { message = "Вакансия не найдена" });
                }

                vacancy.Position = data.Position;
                vacancy.Description = data.Description;
                vacancy.Location = data.Location;
                vacancy.Experience = data.Experience;
                vacancy.SalaryMin = data.SalaryMin;
                vacancy.SalaryMax = data.SalaryMax;
                vacancy.Skills = data.Skills;

                _context.Vacancies.Update(vacancy);
                await _context.SaveChangesAsync();

                return new OkObjectResult(new { message = "Вакансия успешно обновлена", vacancyId = vacancy.Id });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { message = $"Ошибка при обновлении вакансии: {ex.Message}" });
            }
        }

        public async Task<IActionResult> DeleteVacancy(int vacancyId)
        {
            try
            {
                var vacancy = await _context.Vacancies.FindAsync(vacancyId);
                if (vacancy == null)
                {
                    return new NotFoundObjectResult(new { message = "Вакансия не найдена" });
                }

                _context.Vacancies.Remove(vacancy);
                await _context.SaveChangesAsync();

                return new OkObjectResult(new { message = "Вакансия успешно удалена" });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { message = $"Ошибка при удалении вакансии: {ex.Message}" });
            }
        }

        public async Task<IActionResult> UpdateProfile(CompanyRegister data)
        {
            try
            {
                // В реальном приложении companyId должен получаться из контекста (токена)
                // Для примера берем первую компанию
                var company = await _context.Companies
                    .Include(c => c.User)
                    .FirstOrDefaultAsync();

                if (company == null)
                {
                    return new NotFoundObjectResult(new { message = "Компания не найдена" });
                }

                // Обновляем данные компании
                company.Name = data.Name;
                company.Description = data.Description;
                company.Location = data.Location;
                company.Category = data.Category;

                // Обновляем данные пользователя (email)
                if (company.User != null)
                {
                    company.User.Username = data.email;
                    // Пароль обновляется только если предоставлен новый
                    if (!string.IsNullOrEmpty(data.password))
                    {
                        company.User.Password = data.password; // В реальном приложении - хэширование!
                    }
                }

                _context.Companies.Update(company);
                await _context.SaveChangesAsync();

                return new OkObjectResult(new { message = "Профиль успешно обновлен", companyId = company.Id });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { message = $"Ошибка при обновлении профиля: {ex.Message}" });
            }
        }
    }
}