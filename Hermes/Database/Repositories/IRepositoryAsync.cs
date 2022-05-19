using System.Collections.Generic;
using System.Threading.Tasks;

using Hermes.Models;

namespace Hermes.Database.Repositories
{
    public interface IRepositoryAsync<T> where T : BaseModel
    {
        Task<T> GetByIdAsync(long id);

        Task<T> CreateAsync(T entity);

        Task<bool> UpdateAsync(T entity);

        Task<bool> DeleteAsync(long id);

        Task<bool> DeleteAsync(IEnumerable<T> entities);
    }
}