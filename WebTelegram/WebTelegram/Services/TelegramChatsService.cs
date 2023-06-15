using AutoMapper;
using WebTelegram.Abstract;
using WebTelegram.Data;
using WebTelegram.Data.Entities;
using WebTelegram.Models.Cities;
using WebTelegram.Models.Exclusions;
using WebTelegram.Models.TelegramChats;

namespace WebTelegram.Services
{
    public class TelegramChatsService : ITelegramChatsService
    {
        private readonly AppEFContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public TelegramChatsService(AppEFContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public IQueryable<TelegramChatEntity> TelegramChats => _context.TelegramChats;

      
        public async Task<int> Create(CreateTelegramChatViewModel model)
        {
            if (model != null)
            {
                var entity = _mapper.Map<TelegramChatEntity>(model);
                _context.TelegramChats.Add(entity);
                await _context.SaveChangesAsync();
                return entity.Id;
            }
            throw new Exception("Parameter 'entity' can not be null!");
        }

        public async Task<bool> Delete(int id)
        {
            var entity = _context.TelegramChats.SingleOrDefault(x => x.Id == id);
            if (entity != null)
            {
                var data = _context.TelegramChats.SingleOrDefault(p => p.Id == entity.Id);
                data.IsDeleted = true;
                await _context.SaveChangesAsync();
                return data.IsDeleted;
            }
            throw new Exception("In 'id' found 0 entities!");
        }

        public async Task Update(EditTelegramChatViewModel model)
        {
            if (model == null)
                throw new Exception("Parameter 'model' can not be null!");

            var data = _context.TelegramChats
                .SingleOrDefault(x => x.Id == model.Id);

            data.Username = model.Username;
            data.FirstName = model.FirstName;
            data.LastName = model.LastName;

            await _context.SaveChangesAsync();
        }
    }
}
