using EasyIce.Mobile.ViewModels;

namespace EasyIce.Mobile.Views;

public partial class ExercisePlayerPage : ContentPage
{
	public ExercisePlayerPage(ExercisePlayerViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
