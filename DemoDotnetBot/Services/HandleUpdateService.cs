using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DemoDotnetBot.Services
{
    public class HandleUpdateService
    {
        private readonly ILogger<HandleUpdateService> _logger;
        private readonly ITelegramBotClient _botClient;

        public HandleUpdateService(ILogger<HandleUpdateService> logger, ITelegramBotClient botClient)
        {
            _logger = logger;
            _botClient = botClient;
        }

        public async Task EchoAsync(Update update)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageRecieved(update.Message),
                UpdateType.CallbackQuery => BotOnCallBackQueryRecieved(update.CallbackQuery),
                _ => UnknownUpdateTypeHandler(update)
            };

            try
            {
                await handler;
            }
            catch (Exception ex)
            {
                await HandlerErrorAsync(ex);
            }

        }

        private async Task BotOnMessageRecieved(Message message)
        {
            _logger.LogInformation($"Message keldi: {message.Type}");

            await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Botga xabar keldi");
        }

        private async Task BotOnCallBackQueryRecieved(CallbackQuery callbackQuery)
        {
            await _botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text : $"{callbackQuery.Data}");
        }

        private Task UnknownUpdateTypeHandler(Update update)
        {
            _logger.LogInformation($"Unkown update type : {update.Type}");

            return Task.CompletedTask;
        }

        public Task HandlerErrorAsync(Exception ex)
        {
            var ErrorMessage = ex switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n{apiRequestException.ErrorCode}",
                _ => ex.ToString()
            };

            _logger.LogInformation(ErrorMessage);

            return Task.CompletedTask;
        }
    }
}
