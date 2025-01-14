
using HikiAlarmMuti.Services;
using Microsoft.Extensions.Logging;
using HikiAlarmMuti.ViewModels;

/* 项目“HikiAlarmMuti (net8.0-android)”的未合并的更改
添加项:
using HikiAlarmMobile;
using HikiAlarmMuti;
*/




#if ANDROID
using HikiAlarmMuti.Platforms.Android;
#endif
#if WINDOWS
using HikiAlarmMuti.Platforms.Windows;
#endif
namespace HikiAlarmMuti;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Services.AddSingleton<MainViewModel>();
#if WINDOWS
        builder.Services.AddSingleton<IAlarmPlayerService, AlarmPlayerService>();
        builder.Services.AddSingleton<INotificationService, NotificationService>();
#endif
        builder.Services.AddSingleton<MainPage>();
#if ANDROID
        builder.Services.AddSingleton<IAlarmPlayerService, AlarmPlayerService>();
        builder.Services.AddSingleton<INotificationService, NotificationService>();
#endif
        return builder.Build();
    }
}
