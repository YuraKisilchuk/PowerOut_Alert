using AutoMapper;
using WebTelegram.Abstract;
using WebTelegram.Data;
using WebTelegram.Data.Entities;
using WebTelegram.Models.Cities;

namespace WebTelegram.Services
{
    public class CitiesService : ICitiesService
    {
        private readonly AppEFContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public CitiesService(AppEFContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public IQueryable<CityEntity> Cities => _context.Cities;

      
        public async Task<int> Create(CreateCityViewModel model)
        {
            if (model != null)
            {
                var entity = _mapper.Map<CityEntity>(model);
                _context.Cities.Add(entity);
                await _context.SaveChangesAsync();
                return entity.Id;
            }
            throw new Exception("Parameter 'entity' can not be null!");
        }

        public async Task<bool> Delete(int id)
        {
            var entity = _context.Cities.SingleOrDefault(x => x.Id == id);
            if (entity != null)
            {
                var data = _context.Cities.SingleOrDefault(p => p.Id == entity.Id);
                data.IsDeleted = true;
                await _context.SaveChangesAsync();
                return data.IsDeleted;
            }
            throw new Exception("In 'id' found 0 entities!");
        }

        public async Task Update(EditCityViewModel model)
        {
            if (model == null)
                throw new Exception("Parameter 'model' can not be null!");

            var data = _context.Cities
                .SingleOrDefault(x => x.Id == model.Id);

            data.Name = model.Name;
            data.AreaId = model.AreaId;

            await _context.SaveChangesAsync();
        }
    }
}
