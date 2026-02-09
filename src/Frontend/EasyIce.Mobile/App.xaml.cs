using EasyIce.Mobile.Services;
using EasyIce.Mobile.Views;

namespace EasyIce.Mobile;

public partial class App : Application
{
    private readonly AuthService _authService;

	public App(AuthService authService)
	{
		InitializeComponent();
        _authService = authService;
        
		MainPage = new AppShell();
	}

    protected override async void OnStart()
    {
        base.OnStart();
        
        // Simple navigation check
        // Note: In real app, cleaner to use a SplashPage as Root
        bool isAuthenticated = await _authService.IsAuthenticatedAsync();
        
        if (!isAuthenticated)
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
