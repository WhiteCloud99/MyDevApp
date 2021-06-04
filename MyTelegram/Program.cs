using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyTelegram
{
  class Program
  {
    private static readonly string _token = "1640789263:AAHsWxWv5nKxBSvKIoAMOeDsg1aWeWGLkKQ";
    private static readonly TelegramBotClient Bot = new TelegramBotClient(_token);

    public static void Main(string[] args)
    {
      try
      {
        //var me = Bot.GetMeAsync().Result;        
        //Console.Title = me.Username;
        //TelegramAPIAsync();
        Bot.OnMessage += BotOnMessageReceived;
        Bot.OnMessageEdited += BotOnMessageReceived;
        Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
        Bot.OnReceiveError += BotOnReceiveError;
        //Bot.StartReceiving(Array.Empty<UpdateType>());
        Bot.StartReceiving();

        //Console.WriteLine($"Start listening for @{me.Username}");
        Console.WriteLine($"Start listening...");
        Console.ReadLine();

        Bot.StopReceiving();
      }
      catch(Exception ex)
      {
        Console.WriteLine(string.Format("Error: {0}", ex.Message));
      }
      
    }

    private async void TelegramAPIAsync()
    {
      var me = await Bot.GetMeAsync();
    }

    private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
    {
      var message = messageEventArgs.Message;
      if (message == null || message.Type != MessageType.Text) return;
      switch (message.Text)
      {
        case "":
          {
            break;
          }
        default:
          {
            await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Support"),
                }
            });

            await Bot.SendTextMessageAsync(message.Chat.Id, "Main Menu", replyMarkup: inlineKeyboard);
            break;
          }
      }
    }


    private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
    {
      var callbackQuery = callbackQueryEventArgs.CallbackQuery;
      await Bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Received {callbackQuery.Data}");
    }


    private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
    {
      Console.WriteLine("Received error: {0} — {1}",
          receiveErrorEventArgs.ApiRequestException.ErrorCode, receiveErrorEventArgs.ApiRequestException.Message);
    }
  }
}
