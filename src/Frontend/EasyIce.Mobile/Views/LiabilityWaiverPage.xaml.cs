namespace EasyIce.Mobile.Views;

public partial class LiabilityWaiverPage : ContentPage
{
	public LiabilityWaiverPage()
	{
		InitializeComponent();
	}

    private async void OnAcceptClicked(object sender, EventArgs e)
    {
        // Save acceptance
        Preferences.Set("waiver_accepted", true);

        // Navigate to Main
        await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
    }
}
