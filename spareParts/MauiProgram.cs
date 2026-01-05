using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Maui;
namespace spareParts;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        //builder.Services.AddSingleton<IDatabaseService, MySqlService>();

        //builder.Services.AddSingleton(new HttpClient
        //{
        //    BaseAddress = new Uri("http://localhost:5234")
        //});
        //builder.Services.AddTransient<ViewModels.Authentication>();

        return builder.Build();
    }
}
