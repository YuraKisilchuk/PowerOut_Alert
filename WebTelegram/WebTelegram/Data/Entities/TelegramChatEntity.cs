using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTelegram.Data.Entities
{
    [Table("tblTelegramChates")]
    public class TelegramChatEntity : BaseEntity<int>
    {
        public long ChatId { get; set; }
        [Required, StringLength(255)]
        public string FirstName { get; set; }
        [StringLength(255)]
        public string LastName { get; set; }
        [StringLength(255)]
        public string Username { get; set; }
        public virtual ICollection<SubscriptionEntity> Subscriptions { get; set; }
    }
}
