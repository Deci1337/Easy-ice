namespace MauiApp1;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(Views.ExercisePlayerPage), typeof(Views.ExercisePlayerPage));
        Routing.RegisterRoute(nameof(Views.LoginPage), typeof(Views.LoginPage));
        Routing.RegisterRoute(nameof(Views.LiabilityWaiverPage), typeof(Views.LiabilityWaiverPage));
        Routing.RegisterRoute(nameof(Views.MainPage), typeof(Views.MainPage));
	}
}
