using CyberStoreSVC.Mediator.Common;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace CyberStoreSVC.Mediator.Commands.Notify
{
    public sealed class SendTextMessageTelegramBotCommand : ICommand<Unit>
    {
        public long ChatId { get; set; } = -1002357176977;
        public string Message { get; set; } = "";
    }

    public sealed class SendTextMessageTelegramBotCommandHandler : ICommandHandler<SendTextMessageTelegramBotCommand, Unit>
    {
        private readonly ITelegramBotClient _botClient;

        public SendTextMessageTelegramBotCommandHandler( ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        [Obsolete]
        public async Task<Unit> Handle(SendTextMessageTelegramBotCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _botClient.SendTextMessageAsync(
                        chatId: request.ChatId,
                        text: request.Message,
                        parseMode: ParseMode.Html,
                        cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error SendTextMessageTelegramBot::{ex.Message}");
            }

            return Unit.Value;
        }
    }

}

