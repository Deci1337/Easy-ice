namespace EasyIce.Mobile.DTOs;

// Matching Backend DTOs
public record TrainingProgramDto(
    Guid Id,
    string Title,
    string Category,
    int DifficultyLevel,
    bool IsPremium,
    bool IsAccessible,
    int OrderIndex
);

public record ProgramDetailDto(
    Guid Id,
    string Title,
    string Category,
    int DifficultyLevel,
    bool IsPremium,
    List<ExerciseDto> Exercises
);

public record ExerciseDto(
    Guid Id,
    string Title,
    string Description,
    string VideoUrl,
    string PreviewUrl,
    int DurationSeconds,
    string SafetyWarning,
    string Status, // Locked, Available, Completed
    bool IsNext
);

public record ReportProgressDto(
    Guid ExerciseId,
    int MetricValue
);

public record ProgressResponseDto(
    bool Success,
    List<Guid> UnlockedExerciseIds
);
