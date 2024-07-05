using InWestyGator.WebDemo.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System;
using InWestyGator.WebDemo.Core.Models;

namespace InWestyGator.WebDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacksController : ControllerBase
    {
        private readonly IPackService _packService;

        public PacksController(IPackService packService)
        {
            _packService = packService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string id)
        {
            var result = await _packService.GetPackWithHierarchyAsync(id);
            if (result == null || !result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            // TODO: configure these properly
            var requestedPageNumber = pageNumber ?? 1;
            var requestedPageSize = pageSize ?? 1000;

            var result = await _packService.GetPaginatedPacksAsync(requestedPageNumber, requestedPageSize);
            if (result == null || !result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Pack pack)
        {
            // this is a very basic way to turn exceptions into HTTP errors
            try
            {
                await _packService.AddPackAsync(pack);
                return Ok();
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }
    }
}
