﻿using ExpenseControl.Services;
using Microsoft.Extensions.Logging;

namespace ExpenseControl
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

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            string dbPath = FileAcessHelper.GetLocalFilePath("expenses.db3");
            Console.WriteLine($"Database path: {dbPath}");
            builder.Services.AddSingleton<PersonRepository>(s => 
                ActivatorUtilities.CreateInstance<PersonRepository>(s, dbPath));

            return builder.Build();
        }
    }
}
