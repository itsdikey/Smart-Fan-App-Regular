using Microsoft.Extensions.Logging;
using SFAR.ViewModels.Installers;
using SFAR.Services.Installers;
using SFAR.Installers;
using CommunityToolkit.Maui;
using Shiny;
using Microsoft.Maui.LifecycleEvents;
using SFAR.Services;

namespace Smart_Fan_App_Regular
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseShiny()
                 .ConfigureLifecycleEvents(events =>
                 {
#if ANDROID
                    events.AddAndroid(android => android
                        .OnActivityResult((activity, requestCode, resultCode, data) => LogEvent(nameof(AndroidLifecycle.OnActivityResult), requestCode.ToString()))
                        .OnStart((activity) => LogEvent(nameof(AndroidLifecycle.OnStart)))
                        .OnCreate((activity, bundle) => LogEvent(nameof(AndroidLifecycle.OnCreate)))
                        .OnBackPressed((activity) => LogEvent(nameof(AndroidLifecycle.OnBackPressed)))
                        .OnStop((activity) => LogEvent(nameof(AndroidLifecycle.OnStop))));
#endif
                     static bool LogEvent(string eventName, string type = null)
                     {
                         LifecycleEventManager.OnLifeCycleEvent(eventName);
                         //if (eventName == "OnStart")
                         //{
                         //    Shell.Current.GoToAsync("//MainPage");
                         //}
                         System.Diagnostics.Debug.WriteLine($"Lifecycle event: {eventName}{(type == null ? string.Empty : $" ({type})")}");
                         return true;
                     }
                 })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddBluetoothLE();

            builder.Services.InstallSFARServices();
            builder.Services.InstallSFARViewModels();
            builder.Services.InstallSFARViews();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
