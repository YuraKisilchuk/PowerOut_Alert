using WebTelegram.Data.Entities;
using WebTelegram.Models.Exclusions;
using WebTelegram.Models.TelegramChats;

namespace WebTelegram.Abstract
{
    public interface ITelegramChatsService
    {
        IQueryable<TelegramChatEntity> TelegramChats { get; }
        Task<int> Create(CreateTelegramChatViewModel entity);
        Task Update(EditTelegramChatViewModel entity);
        Task<bool> Delete(int id);
    }
}
