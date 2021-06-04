using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace MySimpleTelegram
{
  public partial class Form1 : Form
  {
    private static readonly string _token = "1640789263:AAHsWxWv5nKxBSvKIoAMOeDsg1aWeWGLkKQ";
    private TelegramBotClient Bot = new TelegramBotClient(_token);

    public Form1()
    {
      InitializeComponent();
      setTelegramEvent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      telegramAPIAsync();
    }

    private async void telegramAPIAsync()
    {
      var me = await Bot.GetMeAsync();
      Console.WriteLine("내 이름은 {0}", me.FirstName);
    }

    private void setTelegramEvent()
    {
      Bot.OnMessage += Bot_OnMessage;
      Bot.OnMessageEdited += BotOnMessageReceived;
      Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
      Bot.OnReceiveError += BotOnReceiveError;

      Bot.StartReceiving();
    }

    // Event OnMessage
    private async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
    {
      var msg = e.Message;
      if (msg == null || msg.Type != MessageType.Text)
        return;
      

      Console.WriteLine("{0} 로부터 받은 메시지 : {1}", msg.Chat.Id, msg.Text);

      switch (msg.Text)
      {
        case "사진":
          {
            BotSendImage(msg.Chat.Id.ToString());
            break;
          }
        default:
          {
            await Bot.SendTextMessageAsync(msg.Chat.Id, msg.Text);
            
            //await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            //var inlineKeyboard = new InlineKeyboardMarkup(new[]
            //{
            //    new []
            //    {
            //        InlineKeyboardButton.WithCallbackData("Support"),
            //    }
            //});

            //await Bot.SendTextMessageAsync(message.Chat.Id, "Main Menu", replyMarkup: inlineKeyboard);
            break;
          }
      }
    }


    private void button1_Click(object sender, EventArgs e)
    {
      Bot.SendTextMessageAsync(textBox1.Text, richTextBox1.Text);
    }

    private async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
    {
      var message = messageEventArgs.Message;
      if (message == null || message.Type != MessageType.Text) return;
      switch (message.Text)
      {
        case "사진":
          {
            BotSendImage(message.Chat.Id.ToString());
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


    private async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
    {
      var callbackQuery = callbackQueryEventArgs.CallbackQuery;
      await Bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Received {callbackQuery.Data}");
    }


    private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
    {
      Console.WriteLine("Received error: {0} — {1}",
          receiveErrorEventArgs.ApiRequestException.ErrorCode, receiveErrorEventArgs.ApiRequestException.Message);
    }


    //
    private void BotSendImage(string chatId)
    {
      string imageUri = "https://homepages.cae.wisc.edu/~ece533/images/airplane.png";
      InputOnlineFile iof = new InputOnlineFile(imageUri);
      string caption = "this is a test photo..!";
      Bot.SendPhotoAsync(chatId, iof, caption);
    }
  }
}
