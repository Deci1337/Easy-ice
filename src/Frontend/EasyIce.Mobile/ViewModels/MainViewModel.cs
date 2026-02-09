using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyIce.Mobile.Services;
using EasyIce.Mobile.DTOs;
using System.Collections.ObjectModel;

namespace EasyIce.Mobile.ViewModels;

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
        // Загрузка данных при старте
        // В реальном приложении лучше вызывать в OnNavigatedTo
        LoadProgramsAsync();
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
        // Проверка локального состояния "Открыто" (из DTO)
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

        // Передаем ID программы (в MVP можно упростить и открыть сразу первый урок)
        // В полной версии мы бы шли на страницу деталей программы
        
        // Для демонстрации API: загрузим детали и откроем первый урок
        var details = await _apiService.GetProgramDetailsAsync(program.Id);
        if (details != null && details.Exercises.Any())
        {
            var firstExercise = details.Exercises.First();
            
            // Навигация с передачей параметров
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
        // Логика блокировки: IsAccessible && (PreviousCompleted - это на сервере считается)
        // Для списка программ IsAccessible означает "доступно по подписке"
        IsUnlocked = dto.IsAccessible; 
    }
}
