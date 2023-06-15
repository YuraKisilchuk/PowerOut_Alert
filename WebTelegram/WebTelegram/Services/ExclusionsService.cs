using AutoMapper;
using WebTelegram.Abstract;
using WebTelegram.Data;
using WebTelegram.Data.Entities;
using WebTelegram.Models.Cities;
using WebTelegram.Models.Exclusions;

namespace WebTelegram.Services
{
    public class ExclusionsService : IExclusionsService
    {
        private readonly AppEFContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public ExclusionsService(AppEFContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public IQueryable<ExclusionEntity> Exclusions => _context.Exclusions;

      
        public async Task<int> Create(CreateExclusionViewModel model)
        {
            if (model != null)
            {
                var entity = _mapper.Map<ExclusionEntity>(model);
                _context.Exclusions.Add(entity);
                await _context.SaveChangesAsync();
                return entity.Id;
            }
            throw new Exception("Parameter 'entity' can not be null!");
        }

        public async Task<bool> Delete(int id)
        {
            var entity = _context.Exclusions.SingleOrDefault(x => x.Id == id);
            if (entity != null)
            {
                var data = _context.Exclusions.SingleOrDefault(p => p.Id == entity.Id);
                data.IsDeleted = true;
                await _context.SaveChangesAsync();
                return data.IsDeleted;
            }
            throw new Exception("In 'id' found 0 entities!");
        }

        public async Task Update(EditExclusionViewModel model)
        {
            if (model == null)
                throw new Exception("Parameter 'model' can not be null!");

            var data = _context.Exclusions
                .SingleOrDefault(x => x.Id == model.Id);

            data.Name = model.Name;
            data.CityId = model.CityId;

            await _context.SaveChangesAsync();
        }
    }
}
