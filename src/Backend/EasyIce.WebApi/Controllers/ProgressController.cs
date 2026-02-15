using EasyIce.WebApi.Data;
using EasyIce.WebApi.DTOs;
using EasyIce.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyIce.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProgressController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProgressController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Отправить отчет о завершении упражнения.
    /// </summary>
    /// <param name="report">Данные отчета.</param>
    /// <returns>Результат и список открытых упражнений.</returns>
    [HttpPost]
    public async Task<ActionResult<ProgressResponseDto>> PostProgress([FromBody] ReportProgressDto report)
    {
        // 1. Найти упражнение
        var exercise = await _context.Exercises
            .Include(e => e.Program)
            .FirstOrDefaultAsync(e => e.Id == report.ExerciseId);

        if (exercise == null)
        {
            return NotFound("Упражнение не найдено.");
        }

        // 2. Имитация пользователя (в реальности берем из токена)
        // var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var userId = Guid.Empty; // Заглушка
        // Если юзера нет, найдем первого попавшегося или создадим (для теста)
        var user = await _context.Users.FirstOrDefaultAsync();
        if (user == null) {
             user = new User { Email = "test@example.com", DisplayName="Tester" };
             _context.Users.Add(user);
             await _context.SaveChangesAsync();
        }
        userId = user.Id;

        // 3. Записать/Обновить прогресс
        var progress = await _context.UserProgress
            .FirstOrDefaultAsync(up => up.UserId == userId && up.ExerciseId == report.ExerciseId);

        if (progress == null)
        {
            progress = new UserProgress
            {
                UserId = userId,
                ExerciseId = report.ExerciseId,
                Status = "Completed",
                CompletedAt = DateTime.UtcNow
            };
            _context.UserProgress.Add(progress);
        }
        else
        {
            progress.Status = "Completed";
            progress.CompletedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        // 4. Проверить, какие следующие упражнения открылись
        // Находим упражнения, которые требуют выполнения текущего (report.ExerciseId)
        // Для этого ищем ExerciseRequirements, где RequiredExerciseId == report.ExerciseId
        
        var unlockedIds = new List<Guid>();
        
        // Получаем все упражнения, которые зависят от этого
        var potentialUnlocks = await _context.ExerciseRequirements
            .Where(r => r.RequiredExerciseId == report.ExerciseId)
            .Select(r => r.BlockedExerciseId)
            .ToListAsync();

        foreach (var blockedExerciseId in potentialUnlocks)
        {
            // Проверяем, выполнены ли у них ВСЕ требования
            var allRequirements = await _context.ExerciseRequirements
                .Where(r => r.BlockedExerciseId == blockedExerciseId)
                .Select(r => r.RequiredExerciseId)
                .ToListAsync();

            var completedCount = await _context.UserProgress
                .Where(up => up.UserId == userId && up.Status == "Completed" && allRequirements.Contains(up.ExerciseId))
                .CountAsync();

            if (completedCount == allRequirements.Count)
            {
                // Все требования выполнены -> открываем доступ
                // Создаем запись 'Available' если её еще нет
                var existingNext = await _context.UserProgress
                     .FirstOrDefaultAsync(up => up.UserId == userId && up.ExerciseId == blockedExerciseId);

                if (existingNext == null)
                {
                    _context.UserProgress.Add(new UserProgress
                    {
                        UserId = userId,
                        ExerciseId = blockedExerciseId,
                        Status = "Available"
                    });
                    unlockedIds.Add(blockedExerciseId);
                }
                else if (existingNext.Status == "Locked")
                {
                    existingNext.Status = "Available";
                    unlockedIds.Add(blockedExerciseId);
                }
            }
        }

        await _context.SaveChangesAsync();

        return Ok(new ProgressResponseDto(true, unlockedIds));
    }
}
