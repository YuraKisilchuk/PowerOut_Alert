using WebTelegram.Data.Entities;
using WebTelegram.Models.Areas;

namespace WebTelegram.Abstract
{
    public interface IAreasService
    {
        IQueryable<AreaEntity> Areas { get; }
        Task<int> Create(AreaCreateViewModel entity);
        Task Update(AreaEditViewModel entity);
        Task<bool> Delete(int id);
    }
}
