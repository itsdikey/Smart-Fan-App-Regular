using Microsoft.Extensions.Logging;
using SFAR.ViewModels.Installers;
using SFAR.Services.Installers;
using SFAR.Installers;
using CommunityToolkit.Maui;
using Shiny;

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
