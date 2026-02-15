using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Services;
using MauiApp1.DTOs;

namespace MauiApp1.ViewModels;

[QueryProperty(nameof(ExerciseDto), "Exercise")]
public partial class ExercisePlayerViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    [ObservableProperty]
    private ExerciseDto? exerciseDto;

    [ObservableProperty]
    private string title = "";

    [ObservableProperty]
    private string description = "";

    [ObservableProperty]
    private string safetyWarning = "";
    
    [ObservableProperty]
    private string techniqueDescription = "";

    [ObservableProperty]
    private string commonMistakes = "";

    [ObservableProperty]
    private string videoPlaceholderUrl = "";
    
    [ObservableProperty]
    private bool isCompleted;

    [ObservableProperty]
    private double progress = 0.0;

    partial void OnExerciseDtoChanged(ExerciseDto? value)
    {
        if (value != null)
        {
            Title = value.Title;
            Description = value.Description ?? "Описание загружается...";
            TechniqueDescription = value.Description ?? "Держите равновесие, смотрите вперёд. Колени слегка согнуты.";
            CommonMistakes = "Не смотрите вниз, не заваливайтесь назад, не разводите руки слишком широко.";
            SafetyWarning = value.SafetyWarning ?? "Будьте осторожны.";
            VideoPlaceholderUrl = value.PreviewUrl ?? "";
            IsCompleted = value.Status == "Completed";
            Progress = IsCompleted ? 1.0 : 0.0;
        }
    }

    public ExercisePlayerViewModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    [RelayCommand]
    private async Task CompleteExercise()
    {
        if (ExerciseDto == null) return;

        var result = await _apiService.ReportProgressAsync(ExerciseDto.Id);

        if (result != null && result.Success)
        {
            IsCompleted = true;
            Progress = 1.0;

            await Shell.Current.DisplayAlert("Отличная работа!", "Упражнение выполнено. Следующий уровень разблокирован!", "Вперед");

            if (result.UnlockedExerciseIds.Any())
            {
                 // Logic to highlight next level
            }

            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await Shell.Current.DisplayAlert("Ошибка", "Не удалось сохранить прогресс. Проверьте интернет.", "ОК");
        }
    }
}
