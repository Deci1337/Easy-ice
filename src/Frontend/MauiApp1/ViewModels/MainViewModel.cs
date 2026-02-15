using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Services;
using MauiApp1.DTOs;
using System.Collections.ObjectModel;

namespace MauiApp1.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    [ObservableProperty]
    private bool isSafetyConfirmed;

    [ObservableProperty]
    private string safetyMessage = "Перед выходом на лед проверьте шнуровку коньков и наденьте защиту.";

    [ObservableProperty]
    private int trainingStreak = 5;

    public ObservableCollection<TrainingProgramViewModel> Programs { get; } = new();

    public MainViewModel(ApiService apiService)
    {
        _apiService = apiService;
        _ = LoadProgramsAsync();
    }

    [RelayCommand]
    private async Task LoadProgramsAsync()
    {
        var dtos = await _apiService.GetProgramsAsync();
        Programs.Clear();
        foreach (var dto in dtos)
        {
            Programs.Add(new TrainingProgramViewModel(dto));
        }
    }

    [RelayCommand]
    private async Task OpenProgram(TrainingProgramViewModel program)
    {
        if (!program.IsUnlocked)
        {
            await Shell.Current.DisplayAlert("Доступ закрыт", "Сначала пройдите предыдущий уровень.", "ОК");
            return;
        }

        if (!IsSafetyConfirmed)
        {
             await Shell.Current.DisplayAlert("Безопасность", "Пожалуйста, подтвердите готовность в виджете безопасности.", "ОК");
             return;
        }

        var details = await _apiService.GetProgramDetailsAsync(program.Id);
        if (details != null && details.Exercises.Any())
        {
            var firstExercise = details.Exercises.First();
            
            var navigationParameter = new Dictionary<string, object>
            {
                { "Exercise", firstExercise }
            };
            await Shell.Current.GoToAsync(nameof(Views.ExercisePlayerPage), navigationParameter);
        }
    }
}

public class TrainingProgramViewModel
{
    public Guid Id { get; }
    public string Title { get; }
    public string Category { get; }
    public string Difficulty { get; }
    public bool IsUnlocked { get; }

    public TrainingProgramViewModel(TrainingProgramDto dto)
    {
        Id = dto.Id;
        Title = dto.Title;
        Category = dto.Category;
        Difficulty = $"Level {dto.DifficultyLevel}";
        IsUnlocked = dto.IsAccessible; 
    }
}
