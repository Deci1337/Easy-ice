using System.Net.Http.Json;
using EasyIce.Mobile.DTOs;
using System.Text.Json;

namespace EasyIce.Mobile.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://10.0.2.2:5000/api"; // Emulator localhost alias for Android
    // For iOS device: http://<your-pc-ip>:5000/api

    private List<TrainingProgramDto>? _cachedPrograms;

    public ApiService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl),
            Timeout = TimeSpan.FromSeconds(10)
        };
    }

    /// <summary>
    /// Получить список программ.
    /// Использует простое кэширование (in-memory) при ошибке сети.
    /// </summary>
    public async Task<List<TrainingProgramDto>> GetProgramsAsync(string? category = null)
    {
        try
        {
            var url = "programs";
            if (!string.IsNullOrEmpty(category))
                url += $"?category={category}";

            var response = await _httpClient.GetFromJsonAsync<List<TrainingProgramDto>>(url);
            
            if (response != null)
            {
                _cachedPrograms = response; // Обновляем кэш
                return response;
            }
        }
        catch (Exception ex)
        {
            // Ошибка сети (нет интернета на катке)
            Console.WriteLine($"Network error: {ex.Message}");
            
            // Если есть кэш, возвращаем его
            if (_cachedPrograms != null)
            {
                if (!string.IsNullOrEmpty(category))
                    return _cachedPrograms.Where(p => p.Category == category).ToList();
                return _cachedPrograms;
            }

            // TODO: Здесь можно загрузить данные из локальной БД (SQLite)
        }

        return new List<TrainingProgramDto>(); // Пустой список или выброс исключения
    }

    /// <summary>
    /// Получить детали программы.
    /// </summary>
    public async Task<ProgramDetailDto?> GetProgramDetailsAsync(Guid programId)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ProgramDetailDto>($"programs/{programId}");
        }
        catch
        {
            // Обработка ошибки
            return null; 
        }
    }

    /// <summary>
    /// Отправить прогресс выполнения.
    /// </summary>
    public async Task<ProgressResponseDto?> ReportProgressAsync(Guid exerciseId)
    {
        try
        {
            var report = new ReportProgressDto(exerciseId, 1); // 1 = выполнено
            var response = await _httpClient.PostAsJsonAsync("progress", report);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProgressResponseDto>();
            }
        }
        catch (Exception ex)
        {
             Console.WriteLine($"Error reporting progress: {ex.Message}");
             // TODO: Сохранить в очередь на отправку (Offline Sync)
        }

        return null; // Fail
    }
}
