using MauiApp1.ViewModels;

namespace MauiApp1.Views;

public partial class ExercisePlayerPage : ContentPage
{
	public ExercisePlayerPage(ExercisePlayerViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
