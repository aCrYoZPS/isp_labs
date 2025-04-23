using Lab1.Services;
using Microsoft.Extensions.Logging;

namespace Lab1
{
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

            var services = builder.Services;
            services.AddTransient<IDbService, SQLiteService>();
            services.AddHttpClient<IRateService, RateService>(opt =>
                opt.BaseAddress = new Uri("https://www.nbrb.by/api/exrates/rates"));

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}