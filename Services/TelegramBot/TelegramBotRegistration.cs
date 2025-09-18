using Telegram.Bot;

namespace CyberStoreSVC.Services.TelegramBot
{
    public static class TelegramBotRegistration
	{
        private static readonly string telegramBotToken = "telegramBotToken";
        public static IServiceCollection AddTelegramBotServices(this IServiceCollection services)
        {
            services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(telegramBotToken));
            return services;
        }
    }
}

