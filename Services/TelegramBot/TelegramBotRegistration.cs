using Telegram.Bot;

namespace DotnetApiPostgres.Api.Services.TelegramBot
{
    public static class TelegramBotRegistration
	{
        private static readonly string telegramBotToken = "7943487167:AAEVML1jEvj-lB2--try2QJfW0xiGp531xY";
        public static IServiceCollection AddTelegramBotServices(this IServiceCollection services)
        {
            services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(telegramBotToken));
            return services;
        }
    }
}

