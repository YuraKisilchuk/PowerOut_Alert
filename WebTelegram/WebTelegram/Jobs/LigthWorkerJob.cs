using Microsoft.EntityFrameworkCore;
using Quartz;
using System.Globalization;
using Telegram.Bot;
using WebTelegram.Data;

namespace WebTelegram.Jobs
{
    [DisallowConcurrentExecution]
    public class LigthWorkerJob : IJob
    {
        private readonly AppEFContext _context;
        private readonly IConfiguration _config;
        public LigthWorkerJob(AppEFContext context, IConfiguration config)
        {
            _context = context;
            _config = config;

        }
        public Task Execute(IJobExecutionContext context)
        {
            var subsript = _context.Subscriptions
                .Include(x=>x.Exclusion)
                .Include(x=>x.TelegramChat)
                .Where(x=>x.SendOffInvite==null)
                .Where(x=>x.Exclusion.BeginExclusion< DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))
                .ToList();
            foreach (var sub in subsript)
            {
                var exclusion = sub.Exclusion;
                var begin = exclusion.BeginExclusion.ToString("dd.MM.yyyy HH:mm:ss");
                var end = exclusion.EndExclusion.ToString("dd.MM.yyyy HH:mm:ss");
                string str = "УВАГА "+exclusion.Name + $" ({begin}-{end}) ⌚";
                TelegramBotClient _telegramBot;
                _telegramBot = new TelegramBotClient(_config.GetValue<String>("TelegramToken"));
                _telegramBot.SendTextMessageAsync(sub.TelegramChat.ChatId, str);
                sub.SendOffInvite = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                _context.SaveChanges();
            }

            subsript = _context.Subscriptions
                .Include(x => x.Exclusion)
                .Include(x => x.TelegramChat)
                .Where(x => x.SendOnInvite == null)
                .Where(x => x.Exclusion.EndExclusion < DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))
                .ToList();
            foreach (var sub in subsript)
            {
                var exclusion = sub.Exclusion;
                var begin = exclusion.BeginExclusion.ToString("dd.MM.yyyy HH:mm:ss");
                var end = exclusion.EndExclusion.ToString("dd.MM.yyyy HH:mm:ss");
                string str = "СВІТЛО " + exclusion.Name + $" ({begin}-{end}) ⌚";
                TelegramBotClient _telegramBot;
                _telegramBot = new TelegramBotClient(_config.GetValue<String>("TelegramToken"));
                _telegramBot.SendTextMessageAsync(sub.TelegramChat.ChatId, str);
                sub.SendOnInvite = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                _context.SaveChanges();
            }
            return Task.CompletedTask;
        }
    }
}
