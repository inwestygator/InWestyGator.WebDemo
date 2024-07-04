using InWestyGator.WebDemo.Core.Contracts;
using InWestyGator.WebDemo.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace InWestyGator.WebDemo.DataAccess.Repositories
{
    public class PackRepository : IPackRepository
    {
        private readonly WebDbContext _webDbContext;

        public PackRepository(WebDbContext webDbContext)
        {
            _webDbContext = webDbContext;
        }

        public async Task AddAsync(Pack pack)
        {
            _webDbContext.Packs.Add(pack);
            await _webDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var pack = await GetByIdAsync(id);
            if (pack != null)
            {
                _webDbContext.Remove(pack);
            }
            await _webDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Pack>> GetAsync(Expression<Func<Pack, bool>> predicate)
        {
            var result = await GetPackQuery()
                .Where(predicate)
                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Pack>> GetAsync(Func<IQueryable<Pack>, IQueryable<Pack>> query)
        {
            var resultQuery = query(GetPackQuery());
            var result = await resultQuery.ToListAsync();
            return result;
        }

        public async Task<Pack> GetByIdAsync(string id)
        {
            var result = await GetPackQuery()
                .FirstOrDefaultAsync(p => p.Id == id);
            return result;
        }

        public async Task UpdateAsync(Pack pack)
        {
            _webDbContext.Packs.Update(pack);
            await _webDbContext.SaveChangesAsync();
        }

        private IQueryable<Pack> GetPackQuery()
        {
            return _webDbContext.Packs
                .Include(p => p.Children)
                .Include(p => p.Parent);
        }
    }
}
