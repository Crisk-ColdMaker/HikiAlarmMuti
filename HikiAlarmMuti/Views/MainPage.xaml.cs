namespace HikiAlarmMuti.Views;
using HikiAlarmMuti.ViewModels;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
