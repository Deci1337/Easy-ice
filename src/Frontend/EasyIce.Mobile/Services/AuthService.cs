using System.Net.Http.Json;
using EasyIce.Mobile.DTOs;

namespace EasyIce.Mobile.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://10.0.2.2:5000/api/auth";

    public AuthService()
    {
        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        try
        {
            var loginRequest = new { Email = email, Password = password };
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (result != null)
                {
                    await SecureStorage.SetAsync("auth_token", result.Token);
                    await SecureStorage.SetAsync("user_skill", result.SkillLevel);
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login error: {ex.Message}");
        }
        return false;
    }

    public async Task<bool> RegisterAsync(string email, string password, string name, string skill)
    {
        try
        {
            var request = new { Email = email, Password = password, DisplayName = name, SkillLevel = skill };
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/register", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>(); // Reuse response structure
                if (result != null)
                {
                    await SecureStorage.SetAsync("auth_token", result.Token);
                    await SecureStorage.SetAsync("user_skill", result.SkillLevel);
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Register error: {ex.Message}");
        }
        return false;
    }

    public void Logout()
    {
        SecureStorage.Remove("auth_token");
        SecureStorage.Remove("user_skill");
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await SecureStorage.GetAsync("auth_token");
        return !string.IsNullOrEmpty(token);
    }
}

public class LoginResponse
{
    public string Token { get; set; }
    public string SkillLevel { get; set; }
}
