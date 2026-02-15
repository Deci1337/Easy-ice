using Microsoft.EntityFrameworkCore;
using EasyIce.WebApi.Models;

namespace EasyIce.WebApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<TrainingProgram> Programs { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<UserProgress> UserProgress { get; set; }
    public DbSet<ExerciseRequirement> ExerciseRequirements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Настройка уникальности Email
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Настройка требований (связь М:М через промежуточную сущность)
        // Упражнение имеет список Requirements, где оно выступает как "Заблокированное"
        modelBuilder.Entity<ExerciseRequirement>()
            .HasOne(r => r.BlockedExercise)
            .WithMany(e => e.Requirements)
            .HasForeignKey(r => r.BlockedExerciseId)
            .OnDelete(DeleteBehavior.Restrict); // Защита от каскадного удаления

        // Настройка прогресса (уникальная пара User + Exercise)
        modelBuilder.Entity<UserProgress>()
            .HasIndex(up => new { up.UserId, up.ExerciseId })
            .IsUnique();
    }
}
