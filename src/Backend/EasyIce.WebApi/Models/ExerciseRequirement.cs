using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyIce.WebApi.Models;

/// <summary>
/// Требование для доступа к упражнению.
/// Реализует связь "Многие-ко-многим" для графа зависимости навыков.
/// </summary>
public class ExerciseRequirement
{
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Упражнение, которое заблокировано.
    /// </summary>
    [ForeignKey(nameof(BlockedExercise))]
    public Guid BlockedExerciseId { get; set; }
    public Exercise BlockedExercise { get; set; } = null!;

    /// <summary>
    /// Упражнение, которое нужно выполнить (пререквизит).
    /// </summary>
    [ForeignKey(nameof(RequiredExercise))]
    public Guid RequiredExerciseId { get; set; }
    public Exercise RequiredExercise { get; set; } = null!;
}
