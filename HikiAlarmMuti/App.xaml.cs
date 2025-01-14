namespace HikiAlarmMuti;
using HikiAlarmMuti.ViewModels;
public partial class App : Application
{
    private IServiceProvider _serviceProvider;
    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;

    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new MainPage(_serviceProvider.GetRequiredService<MainViewModel>()));
    }
}
