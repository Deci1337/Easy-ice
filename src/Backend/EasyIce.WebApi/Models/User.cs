using System.ComponentModel.DataAnnotations;

namespace EasyIce.WebApi.Models;

/// <summary>
/// Пользователь приложения.
/// Хранит учетные данные и профиль.
/// </summary>
public class User
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string Role { get; set; } = "User"; // User, Admin

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Навигационные свойства
    public ICollection<UserProgress> Progress { get; set; } = new List<UserProgress>();

    public string SkillLevel { get; set; } = "Novice"; // Novice, Amateur, HockeyPlayer
}
