using System.Net.Http.Json;
using MauiApp1.DTOs;

namespace MauiApp1.Services;

public class AuthService
{
    private const bool UseMock = true;
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://10.0.2.2:5000/api/auth";

    public AuthService()
    {
        _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        if (UseMock) return await MockLoginAsync(email, password);

        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/login", new { Email = email, Password = password });
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
                if (result != null)
                {
                    await SecureStorage.SetAsync("auth_token", result.Token);
                    await SecureStorage.SetAsync("user_skill", result.SkillLevel);
                    return true;
                }
            }
        }
        catch (Exception ex) { Console.WriteLine($"Login error: {ex.Message}"); }
        return false;
    }

    public async Task<bool> RegisterAsync(string email, string password, string name, string skill)
    {
        if (UseMock) return await MockLoginAsync(email, password);

        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/register", new RegisterRequestDto(email, password, name, skill));
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
                if (result != null)
                {
                    await SecureStorage.SetAsync("auth_token", result.Token);
                    await SecureStorage.SetAsync("user_skill", result.SkillLevel);
                    return true;
                }
            }
        }
        catch (Exception ex) { Console.WriteLine($"Register error: {ex.Message}"); }
        return false;
    }

    public void Logout()
    {
        SecureStorage.Remove("auth_token");
        SecureStorage.Remove("user_skill");
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        if (UseMock) return false;
        var token = await SecureStorage.GetAsync("auth_token");
        return !string.IsNullOrEmpty(token);
    }

    private static async Task<bool> MockLoginAsync(string email, string password)
    {
        await Task.Delay(500);
        if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
        {
            await SecureStorage.SetAsync("auth_token", "mock_token_123");
            await SecureStorage.SetAsync("user_skill", "Beginner");
            return true;
        }
        return false;
    }
}
