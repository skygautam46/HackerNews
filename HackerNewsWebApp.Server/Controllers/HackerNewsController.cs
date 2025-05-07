using HackerNewsWebApp.Core.Entities;
using HackerNewsWebApp.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace HackerNewsWebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HackerNewsController : ControllerBase
    {

        private readonly ILogger<HackerNews> _logger;
        private IMemoryCache _cacheRepository;

        private readonly IHackerNewsRepository _hackerRepository;

        public HackerNewsController(IMemoryCache cacheRepository, IHackerNewsRepository hackerRepository, ILogger<HackerNews> logger)
        {
            _logger = logger;
            this._cacheRepository = cacheRepository;
            this._hackerRepository = hackerRepository;
        }

        [HttpGet(Name = "GetHackerNews")]
        public async Task<List<HackerNews>> Get(string? searchTerm)
        {
            var response = await _hackerRepository.GetFilteredStory(searchTerm);
            return response;
        }
    }
}
