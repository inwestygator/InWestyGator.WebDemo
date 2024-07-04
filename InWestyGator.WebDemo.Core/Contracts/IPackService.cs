using InWestyGator.WebDemo.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InWestyGator.WebDemo.Core.Contracts
{
    public interface IPackService
    {
        Task<IEnumerable<Pack>> GetPackWithHierarchyAsync(string id);
        Task<IEnumerable<Pack>> GetPaginatedPacksAsync(int pageNumber, int pageSize);
        Task AddPackAsync(Pack pack);
    }
}
