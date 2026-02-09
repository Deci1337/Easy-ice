using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyIce.Mobile.Services;
using EasyIce.Mobile.DTOs;

namespace EasyIce.Mobile.ViewModels;

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
    private bool isCompleted;

    [ObservableProperty]
    private double progress = 0.0;

    partial void OnExerciseDtoChanged(ExerciseDto? value)
    {
        if (value != null)
        {
            Title = value.Title;
            Description = value.Description ?? "Описание загружается...";
            SafetyWarning = value.SafetyWarning ?? "Будьте осторожны.";
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

        // Отправка на сервер
        var result = await _apiService.ReportProgressAsync(ExerciseDto.Id);

        if (result != null && result.Success)
        {
            IsCompleted = true;
            Progress = 1.0;

            // Анимация успеха (Просто сообщение, для реальной анимации нужен доступ к View)
            await Shell.Current.DisplayAlert("Отличная работа! 🔥", "Упражнение выполнено. Следующий уровень разблокирован!", "Вперед");

            // Если есть разблокированные уровни - можно показать их
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
