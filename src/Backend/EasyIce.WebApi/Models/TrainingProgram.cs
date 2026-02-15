using System.ComponentModel.DataAnnotations;

namespace EasyIce.WebApi.Models;

/// <summary>
/// Программа тренировок (курс).
/// Группирует упражнения по темам (Лед, ОФП, Растяжка).
/// </summary>
public class TrainingProgram
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Категория: Ice, OffIce, Stretching
    /// </summary>
    [Required]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Уровень сложности: 1 - 5
    /// </summary>
    public int DifficultyLevel { get; set; }

    public bool IsPremium { get; set; }

    public int OrderIndex { get; set; }

    // Навигационные свойства
    public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
}
