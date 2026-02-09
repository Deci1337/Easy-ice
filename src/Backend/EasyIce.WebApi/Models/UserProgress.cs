using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyIce.WebApi.Models;

/// <summary>
/// Прогресс пользователя по конкретному упражнению.
/// </summary>
public class UserProgress
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    [ForeignKey(nameof(Exercise))]
    public Guid ExerciseId { get; set; }
    public Exercise Exercise { get; set; } = null!;

    /// <summary>
    /// Статус: Locked, Available, Completed
    /// </summary>
    [Required]
    public string Status { get; set; } = "Locked";

    public DateTime? CompletedAt { get; set; }
}
