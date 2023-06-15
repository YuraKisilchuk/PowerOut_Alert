using WebTelegram.Data.Entities;
using WebTelegram.Models.Cities;

namespace WebTelegram.Abstract
{
    public interface ICitiesService
    {
        IQueryable<CityEntity> Cities { get; }
        Task<int> Create(CreateCityViewModel entity);
        Task Update(EditCityViewModel entity);
        Task<bool> Delete(int id);
    }
}
