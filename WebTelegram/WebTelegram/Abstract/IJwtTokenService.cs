using WebTelegram.Data.Entities.Identity;

namespace WebTelegram.Abstract
{
    public interface IJwtTokenService
    {
        Task<string> CreateToken(UserEntity user);
    }
}
