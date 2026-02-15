namespace MauiApp1.Views;

public partial class LiabilityWaiverPage : ContentPage
{
	public LiabilityWaiverPage()
	{
		InitializeComponent();
	}

    private async void OnAcceptClicked(object sender, EventArgs e)
    {
        Preferences.Set("waiver_accepted", true);
        await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
    }
}
