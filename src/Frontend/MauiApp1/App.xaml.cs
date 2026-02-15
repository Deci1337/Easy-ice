using MauiApp1.Services;
using MauiApp1.Views;

namespace MauiApp1;

public partial class App : Application
{
    private readonly AuthService _authService;

	public App(AuthService authService)
	{
		InitializeComponent();
        _authService = authService;
	}

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell());
        
        Task.Run(async () =>
        {
            bool isAuthenticated = await _authService.IsAuthenticatedAsync();
            if (!isAuthenticated)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        });

        return window;
    }
}
