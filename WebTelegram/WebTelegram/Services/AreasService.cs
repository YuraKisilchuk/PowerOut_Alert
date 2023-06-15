using AutoMapper;
using WebTelegram.Abstract;
using WebTelegram.Data;
using WebTelegram.Data.Entities;
using WebTelegram.Models.Areas;

namespace WebTelegram.Services
{
    public class AreasService : IAreasService
    {
        private readonly AppEFContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AreasService(AppEFContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public IQueryable<AreaEntity> Areas => _context.Areas;


        public async Task<int> Create(AreaCreateViewModel model)
        {
            if (model != null)
            {
                var entity = _mapper.Map<AreaEntity>(model);
                _context.Areas.Add(entity);
                await _context.SaveChangesAsync();
                return entity.Id;
            }
            throw new Exception("Parameter 'entity' can not be null!");
        }


        public async Task<bool> Delete(int id)
        {
            var entity = _context.Areas.SingleOrDefault(x => x.Id == id);
            if (entity != null)
            {
                var data = _context.Areas.SingleOrDefault(p => p.Id == entity.Id);
                data.IsDeleted = true;
                await _context.SaveChangesAsync();
                return data.IsDeleted;
            }
            throw new Exception("In 'id' found 0 entities!");
        }

        public async Task Update(AreaEditViewModel model)
        {
            if (model == null)
                throw new Exception("Parameter 'model' can not be null!");

            var data = _context.Areas.SingleOrDefault(x => x.Id == model.Id);

            
            data.Name = model.Name;
            await _context.SaveChangesAsync();
        }
    }
}
