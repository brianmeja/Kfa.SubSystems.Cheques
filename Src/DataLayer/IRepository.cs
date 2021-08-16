using Kfa.SubSystems.Cheques.Contracts.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kfa.SubSystems.Cheques.DataLayer
{
    public interface IRepository<T> where T : KfaData, new()
    {
        Task AddAsync(params T[] objects);

        Task DeleteAsync(params T[] objects);

        Task UpdateAsync(params T[] objects);

        Task<IEnumerable<T>> GetAsync(params long[] ids);

        Task<IEnumerable<T>> GetAsync();
    }
}