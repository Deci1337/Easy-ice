using EasyIce.Mobile.Services;

namespace EasyIce.Mobile;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        // Register routes
        Routing.RegisterRoute(nameof(Views.ExercisePlayerPage), typeof(Views.ExercisePlayerPage));
        Routing.RegisterRoute(nameof(Views.LoginPage), typeof(Views.LoginPage));
        Routing.RegisterRoute(nameof(Views.LiabilityWaiverPage), typeof(Views.LiabilityWaiverPage));
        Routing.RegisterRoute(nameof(Views.MainPage), typeof(Views.MainPage));
	}
}
