using EasyIce.WebApi.Data;
using EasyIce.WebApi.DTOs;
using EasyIce.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyIce.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProgramsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProgramsController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Получить список всех программ обучения.
    /// </summary>
    /// <param name="category">Фильтр по категории (Ice, OffIce, Stretching).</param>
    /// <returns>Список программ.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrainingProgramDto>>> GetPrograms([FromQuery] string? category)
    {
        var query = _context.Programs.AsQueryable();

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(p => p.Category == category);
        }

        var programs = await query
            .OrderBy(p => p.OrderIndex)
            .Select(p => new TrainingProgramDto(
                p.Id,
                p.Title,
                p.Category,
                p.DifficultyLevel,
                p.IsPremium,
                !p.IsPremium, // TODO: Реализовать проверку подписки. Пока Premium недоступен.
                p.OrderIndex
            ))
            .ToListAsync();

        // Заглушка, если база пустая (для демонстрации)
        if (!programs.Any())
        {
            return Ok(GetPlaceholderPrograms(category));
        }

        return Ok(programs);
    }

    /// <summary>
    /// Получить детальную информацию о программе и список упражнений.
    /// </summary>
    /// <param name="id">ID программы.</param>
    /// <returns>Детали программы.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProgramDetailDto>> GetProgramById(Guid id)
    {
        var program = await _context.Programs
            .Include(p => p.Exercises)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (program == null)
        {
            return NotFound("Программа не найдена.");
        }

        // В реальном приложении User ID берется из токена
        // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // Здесь используем заглушку пользователя или просто возвращаем статус по умолчанию
        
        var exercisesDto = program.Exercises
            .OrderBy(e => e.OrderIndex)
            .Select(e => new ExerciseDto(
                e.Id,
                e.Title,
                e.Description,
                e.VideoUrl,
                e.PreviewUrl,
                e.DurationSeconds,
                e.SafetyWarning,
                "Locked", // Заглушка статуса. В реальности нужно джойнить UserProgress
                false
            ))
            .ToList();

        // Открываем первый урок
        if (exercisesDto.Any())
        {
             // Логика переопределения статуса (временно здесь)
             // exercisesDto[0] = exercisesDto[0] with { Status = "Available", IsNext = true };
        }

        return Ok(new ProgramDetailDto(
            program.Id,
            program.Title,
            program.Category,
            program.DifficultyLevel,
            program.IsPremium,
            exercisesDto
        ));
    }

    // --- Helpers (Placeholders) ---
    private List<TrainingProgramDto> GetPlaceholderPrograms(string? category)
    {
        var list = new List<TrainingProgramDto>
        {
            new(Guid.NewGuid(), "Базовое скольжение", "Ice", 1, false, true, 1),
            new(Guid.NewGuid(), "Фонарики и Елочка", "Ice", 1, false, true, 2),
            new(Guid.NewGuid(), "ОФП для хоккеистов", "OffIce", 2, false, true, 1),
            new(Guid.NewGuid(), "Растяжка спины", "Stretching", 1, false, true, 1)
        };

        if (!string.IsNullOrEmpty(category))
        {
            return list.Where(p => p.Category == category).ToList();
        }
        return list;
    }
}
