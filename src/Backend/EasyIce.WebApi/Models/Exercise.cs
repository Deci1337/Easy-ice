using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyIce.WebApi.Models;

/// <summary>
/// Упражнение (урок).
/// Единица контента с видео и описанием.
/// </summary>
public class Exercise
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(Program))]
    public Guid ProgramId { get; set; }
    public TrainingProgram Program { get; set; } = null!;

    [Required]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Ссылка на видео (заглушка или CDN).
    /// </summary>
    public string VideoUrl { get; set; } = string.Empty;

    public string PreviewUrl { get; set; } = string.Empty;

    public int DurationSeconds { get; set; }

    public int OrderIndex { get; set; }

    public string SafetyWarning { get; set; } = string.Empty;

    // Навигационные свойства
    
    /// <summary>
    /// Требования для открытия этого упражнения.
    /// Список навыков (других упражнений), которые нужно выполнить.
    /// </summary>
    public ICollection<ExerciseRequirement> Requirements { get; set; } = new List<ExerciseRequirement>();
}
