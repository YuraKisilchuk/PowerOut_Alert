using WebTelegram.Data.Entities;
using WebTelegram.Models.Exclusions;

namespace WebTelegram.Abstract
{
    public interface IExclusionsService
    {
        IQueryable<ExclusionEntity> Exclusions { get; }
        Task<int> Create(CreateExclusionViewModel entity);
        Task Update(EditExclusionViewModel entity);
        Task<bool> Delete(int id);
    }
}
