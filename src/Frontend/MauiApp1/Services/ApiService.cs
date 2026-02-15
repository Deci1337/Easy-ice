using System.Net.Http.Json;
using MauiApp1.DTOs;

namespace MauiApp1.Services;

public class ApiService
{
    private const bool UseMock = true;
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://10.0.2.2:5000/api";
    private List<TrainingProgramDto>? _cachedPrograms;

    private static readonly Guid MockProgramId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid MockExerciseId = Guid.Parse("22222222-2222-2222-2222-222222222222");

    public ApiService()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl), Timeout = TimeSpan.FromSeconds(10) };
    }

    public async Task<List<TrainingProgramDto>> GetProgramsAsync(string? category = null)
    {
        if (UseMock) return await Task.FromResult(GetMockPrograms(category));

        try
        {
            var url = "programs" + (string.IsNullOrEmpty(category) ? "" : $"?category={category}");
            var response = await _httpClient.GetFromJsonAsync<List<TrainingProgramDto>>(url);
            if (response != null) { _cachedPrograms = response; return response; }
        }
        catch { if (_cachedPrograms != null) return category != null ? _cachedPrograms.Where(p => p.Category == category).ToList() : _cachedPrograms; }

        return new List<TrainingProgramDto>();
    }

    public async Task<ProgramDetailDto?> GetProgramDetailsAsync(Guid programId)
    {
        if (UseMock) return await Task.FromResult(GetMockProgramDetails(programId));

        try { return await _httpClient.GetFromJsonAsync<ProgramDetailDto>($"programs/{programId}"); }
        catch { return null; }
    }

    public async Task<ProgressResponseDto?> ReportProgressAsync(Guid exerciseId)
    {
        if (UseMock) return await Task.FromResult(new ProgressResponseDto(true, new List<Guid> { MockExerciseId }));

        try
        {
            var response = await _httpClient.PostAsJsonAsync("progress", new ReportProgressDto(exerciseId, 1));
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<ProgressResponseDto>() : null;
        }
        catch { return null; }
    }

    private static List<TrainingProgramDto> GetMockPrograms(string? category)
    {
        var list = new List<TrainingProgramDto>
        {
            new(MockProgramId, "Основы катания", "Beginner", 1, false, true, 0),
            new(Guid.Parse("33333333-3333-3333-3333-333333333333"), "Продвинутый уровень", "Advanced", 2, false, true, 1)
        };
        return category != null ? list.Where(p => p.Category == category).ToList() : list;
    }

    private static ProgramDetailDto? GetMockProgramDetails(Guid programId) =>
        new(programId, "Основы катания", "Beginner", 1, false, new List<ExerciseDto>
        {
            new(MockExerciseId, "Первые шаги", "Держите равновесие, скользите вперед.", "", "", 120, "Наденьте защиту.", "Available", true)
        });
}
