using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using EasyIce.Mobile.Views;
using EasyIce.Mobile.ViewModels;
using EasyIce.Mobile.Services;

namespace EasyIce.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        // Services
        builder.Services.AddSingleton<ApiService>();
        builder.Services.AddSingleton<AuthService>();

        // Register Pages and ViewModels
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MainViewModel>();
        
        builder.Services.AddTransient<ExercisePlayerPage>();
        builder.Services.AddTransient<ExercisePlayerViewModel>();

        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<LoginViewModel>();

        builder.Services.AddTransient<LiabilityWaiverPage>();

		return builder.Build();
	}
}
