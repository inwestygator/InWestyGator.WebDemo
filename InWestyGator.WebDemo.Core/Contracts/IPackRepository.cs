using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using InWestyGator.WebDemo.Core.Models;
using System.Linq;

namespace InWestyGator.WebDemo.Core.Contracts
{
    public interface IPackRepository
    {
        Task<IEnumerable<Pack>> GetAsync(Func<IQueryable<Pack>, IQueryable<Pack>> query);
        Task<IEnumerable<Pack>> GetAsync(Expression<Func<Pack, bool>> predicate);
        Task<Pack> GetByIdAsync(string id);
        Task AddAsync(Pack pack);
        Task UpdateAsync(Pack pack);
        Task DeleteAsync(string id);
    }
}
