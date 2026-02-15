using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyIce.Mobile.Services;

namespace EasyIce.Mobile.ViewModels;

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
            // Переход к Legal или Main
            // Для MVP: переходим к Legal Waiver, если это первый вход (можно флаг сохранить)
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
