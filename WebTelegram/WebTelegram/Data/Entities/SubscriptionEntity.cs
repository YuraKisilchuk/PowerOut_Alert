using System.ComponentModel.DataAnnotations.Schema;

namespace WebTelegram.Data.Entities
{
    [Table("tblSubscriptions")]
    public class SubscriptionEntity : BaseEntity<int>
    {
        [ForeignKey("Exclusion")]
        public int ExclusionId { get; set; }
        public virtual ExclusionEntity Exclusion { get; set; }

        [ForeignKey("TelegramChat")]
        public int TelegramChatId { get; set; }
        public virtual TelegramChatEntity TelegramChat { get; set; }
        //Дата насилання сповіщення про відключення
        public DateTime ? SendOffInvite { get; set; }
        //Дата насилання сповіщення про включення світла
        public DateTime? SendOnInvite { get; set; }
    }
}
