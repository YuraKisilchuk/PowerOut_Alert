using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WebTelegram.Abstract;
using WebTelegram.Data;
using WebTelegram.Data.Entities;
using WebTelegram.Services;

namespace WebTelegram
{
    public class Worker
    {
        private readonly TelegramBotClient _telegramBot;
        private readonly IApplicationBuilder _app;

        public Worker(IApplicationBuilder app)
        {
            _app = app;
            using (var scope = _app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                _telegramBot = new TelegramBotClient(config.GetValue<String>("TelegramToken"));
                _telegramBot.StartReceiving(Update, Error);
                _telegramBot.SendTextMessageAsync("357940757", "Бот стартанув успішно");
            }
        }

        private async Task Update(ITelegramBotClient botCleint, Update update, CancellationToken token)
        {

            var message = update.Message;
            var callBackQuery = update.CallbackQuery;

            if (message == null || message.Type != MessageType.Text) return;

            //Тільки потрапив у чат
            if(message.Chat!=null)
            {
                using (var scope = _app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppEFContext>();
                    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                    var telegramChat = context.TelegramChats
                        .Where(x=>!x.IsDeleted)
                        .SingleOrDefault(x => x.ChatId == message.Chat.Id);
                    if (telegramChat == null)
                    {
                        telegramChat = mapper.Map<TelegramChatEntity>(message.Chat);
                        context.TelegramChats.Add(telegramChat);
                        context.SaveChanges();
                    }


                }
            }
            
            if (!string.IsNullOrEmpty(message.Text))
            {
                //Показумємо головне меню
                if (message.Text.ToLower().Contains("/start")||message.Text.ToLower().Contains("🔙"))
                {
                    ReplyKeyboardMarkup menu;
                    using (var scope = _app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<AppEFContext>();
                        List<KeyboardButton[]> keyboardButtons = new List<KeyboardButton[]>();
                        foreach (var area in context.Areas.Where(x=>!x.IsDeleted).ToList())
                        {
                            keyboardButtons.Add(new KeyboardButton[] { area.Name + " ®️" });
                        }
                        menu = new ReplyKeyboardMarkup(keyboardButtons)
                        {
                            ResizeKeyboard = true
                        };
                    }

                    Message sentMessage = await _telegramBot.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Оберіть область",
                        replyMarkup: menu,
                        cancellationToken: token);
                }

                //Скасовуємо підписку
                else if (message.Text.ToLower().Contains("✖️"))
                {
                    ReplyKeyboardMarkup menu;
                    using (var scope = _app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<AppEFContext>();
                        var telegramChat = context.TelegramChats.SingleOrDefault(x => x.ChatId == message.Chat.Id);
                        SubscriptionEntity subscription = context.Subscriptions.SingleOrDefault(x => x.TelegramChatId== telegramChat.Id);
                        if(subscription!=null)
                        {
                            context.Subscriptions.Remove(subscription);
                            context.SaveChanges();
                        }

                        List<KeyboardButton[]> keyboardButtons = new List<KeyboardButton[]>();
                        foreach (var area in context.Areas.Where(x => !x.IsDeleted).ToList())
                        {
                            keyboardButtons.Add(new KeyboardButton[] { area.Name + " ®️" });
                        }
                        menu = new ReplyKeyboardMarkup(keyboardButtons)
                        {
                            ResizeKeyboard = true
                        };
                    }

                    Message sentMessage = await _telegramBot.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Оберіть область",
                        replyMarkup: menu,
                        cancellationToken: token);
                }

                else if (message.Text.ToLower().Contains("®️"))
                {
                    string areaName = message.Text.Replace(" ®️", "");
                    ReplyKeyboardMarkup menu;
                    using (var scope = _app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<AppEFContext>();
                        var area = context.Areas.Where(x => !x.IsDeleted).ToList().SingleOrDefault(x => x.Name == areaName);
                        if (area != null)
                        {
                            List<KeyboardButton[]> keyboardButtons = new List<KeyboardButton[]>();
                            foreach (var city in context.Cities.Where(x => !x.IsDeleted)
                                .Where(x=>x.AreaId==area.Id).ToList())
                            {
                                keyboardButtons.Add(new KeyboardButton[] { city.Name + " ⛺" });
                            }
                            keyboardButtons.Add(new KeyboardButton[] { "Повернутися назад" + " 🔙" });
                            menu = new ReplyKeyboardMarkup(keyboardButtons)
                            {
                                ResizeKeyboard = true
                            };
                            Message sentMessage = await _telegramBot.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: "Оберіть місто",
                                replyMarkup: menu,
                                cancellationToken: token);
                        }

                    }
                }
                //підписка на сповідення
                else if (message.Text.ToLower().Contains("⌚"))
                {
                    string text = message.Text;
                    ReplyKeyboardMarkup menu;
                    using (var scope = _app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<AppEFContext>();
                        var list = context.Exclusions.Where(x => !x.IsDeleted).ToList();
                        ExclusionEntity searchExclusion = null;
                        foreach (var exclusion in list)
                        {
                            var begin = exclusion.BeginExclusion.ToString("dd.MM.yyyy HH:mm:ss");
                            var end = exclusion.EndExclusion.ToString("dd.MM.yyyy HH:mm:ss");
                            string str = exclusion.Name + $" ({begin}-{end}) ⌚";
                            if (text == str)
                            {
                                searchExclusion = exclusion;
                                break;
                            }
                        }
                        if(searchExclusion!=null)
                        {
                            var telegramChat = context.TelegramChats.SingleOrDefault(x => x.ChatId == message.Chat.Id);
                            SubscriptionEntity subscription = new SubscriptionEntity
                            {
                                ExclusionId = searchExclusion.Id,
                                TelegramChatId = telegramChat.Id
                            };
                            context.Subscriptions.Add(subscription);
                            context.SaveChanges();
                            List<KeyboardButton[]> keyboardButtons = new List<KeyboardButton[]>();
                            keyboardButtons.Add(new KeyboardButton[] { "Скасувати підписку ✖️" });
                            menu = new ReplyKeyboardMarkup(keyboardButtons)
                            {
                                ResizeKeyboard = true
                            };
                            Message sentMessage = await _telegramBot.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: "Ви підписалися на сповіщення про відлкючення",
                                replyMarkup: menu,
                                cancellationToken: token);

                        }
                    }
                }

                else if (message.Text.ToLower().Contains("⛺"))
                {
                    string cityName = message.Text.Replace(" ⛺", "");
                    ReplyKeyboardMarkup menu;
                    using (var scope = _app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<AppEFContext>();
                        var city = context.Cities.Where(x => !x.IsDeleted).ToList().SingleOrDefault(x => x.Name == cityName);
                        if (city != null)
                        {
                            List<KeyboardButton[]> keyboardButtons = new List<KeyboardButton[]>();
                            foreach (var exclusion in context.Exclusions.Where(x => !x.IsDeleted)
                                //.Where(x=>x.BeginExclusion.Date == DateTime.Now.Date)
                                .Where(x => x.CityId == city.Id).ToList())
                            {
                                var begin = exclusion.BeginExclusion.ToString("dd.MM.yyyy HH:mm:ss");
                                var end = exclusion.EndExclusion.ToString("dd.MM.yyyy HH:mm:ss");
                                keyboardButtons.Add(new KeyboardButton[] { exclusion.Name + $" ({begin}-{end}) ⌚" });
                            }
                            keyboardButtons.Add(new KeyboardButton[] { "Повернутися назад" + " 🔙" });
                            menu = new ReplyKeyboardMarkup(keyboardButtons)
                            {
                                ResizeKeyboard = true
                            };
                            Message sentMessage = await _telegramBot.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: "Оберіть графік відключення",
                                replyMarkup: menu,
                                cancellationToken: token);
                        }

                    }
                }
            
                else
                {
                    await _telegramBot.SendTextMessageAsync(message.Chat.Id, "--Команда не підримується--");
                }
            }
        }
        private Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }
    }
}
