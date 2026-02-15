using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Services;

namespace MauiApp1.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly AuthService _authService;

    [ObservableProperty]
    private string email = "test@test.com";

    [ObservableProperty]
    private string password = "password";

    public LoginViewModel(AuthService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    private async Task Login()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            await Shell.Current.DisplayAlert("Ошибка", "Введите email и пароль", "ОК");
            return;
        }

        var success = await _authService.LoginAsync(Email, Password);
        if (success)
        {
            bool waiverAccepted = Preferences.Get("waiver_accepted", false);
            
            if (!waiverAccepted)
            {
                 await Shell.Current.GoToAsync($"//{nameof(Views.LiabilityWaiverPage)}");
            }
            else
            {
                 await Shell.Current.GoToAsync($"//{nameof(Views.MainPage)}");
            }
        }
        else
        {
            await Shell.Current.DisplayAlert("Ошибка", "Неверный логин или пароль", "ОК");
        }
    }
}
