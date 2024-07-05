using InWestyGator.WebDemo.Core.Contracts;
using InWestyGator.WebDemo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InWestyGator.WebDemo.Business.Services
{
    public class PackService : IPackService
    {
        private readonly IPackRepository _packRepository;

        public PackService(IPackRepository packRepository)
        {
            _packRepository = packRepository;
        }

        public async Task<IEnumerable<Pack>> GetPackWithHierarchyAsync(string id)
        {
            var result = new List<Pack>();

            var rootPack = await _packRepository.GetByIdAsync(id);
            await PopulatePackHierarchy(rootPack, result, new HashSet<string>());

            foreach (var pack in result)
            {
                pack.ChildPackIds = pack.Children?.Select(x => x.Id).ToList();
            }

            return result;
        }

        public async Task<IEnumerable<Pack>> GetPaginatedPacksAsync(int pageNumber, int pageSize)
        {
            Func<IQueryable<Pack>, IQueryable<Pack>> paginationLogic = packs => packs
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var packs = await _packRepository.GetAsync(paginationLogic);

            foreach (var pack in packs)
            {
                pack.ChildPackIds = pack.Children?.Select(x => x.Id).ToList();
            }

            return packs;
        }

        public async Task AddPackAsync(Pack pack)
        {
            pack.Children = new List<Pack>();
            if (pack.ChildPackIds != null)
            {
                foreach (var childId in pack.ChildPackIds)
                {
                    if (childId != pack.Id) // Prevent adding itself as a child
                    {
                        var child = await _packRepository.GetByIdAsync(childId);
                        if (child != null)
                        {
                            pack.Children.Add(child);
                        }
                    }
                }
            }
            await _packRepository.AddAsync(pack);
        }

        private async Task PopulatePackHierarchy(Pack pack, ICollection<Pack> result, HashSet<string> visited)
        {
            if (pack == null || result.Contains(pack) || visited.Contains(pack.Id))
            {
                return;
            }

            result.Add(pack);
            visited.Add(pack.Id);

            if (pack.Children != null)
            {
                foreach (var child in pack.Children)
                {
                    var loadedChild = await _packRepository.GetByIdAsync(child.Id);
                    await PopulatePackHierarchy(loadedChild, result, visited);
                }
            }
        }
    }
}
