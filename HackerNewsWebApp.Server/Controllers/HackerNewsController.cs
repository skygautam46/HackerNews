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
            List<HackerNews> stories = new List<HackerNews>();

            var response = await _hackerRepository.BestStoriesAsync();
            if (response.IsSuccessStatusCode)
            {
                var storiesResponse = response.Content.ReadAsStringAsync().Result;
                var bestIds = JsonConvert.DeserializeObject<List<int>>(storiesResponse);

                var tasks = bestIds.Select(GetStoryAsync);
                stories = (await Task.WhenAll(tasks)).ToList();

                if (!String.IsNullOrEmpty(searchTerm))
                {
                    var search = searchTerm.ToLower();
                    stories = stories.Where(s =>
                                       s.Title.ToLower().IndexOf(search) > -1 || s.By.ToLower().IndexOf(search) > -1)
                                       .ToList();
                }
            }
            return stories;
        }

        private async Task<HackerNews?> GetStoryAsync(int storyId)
        {
            return await _cacheRepository.GetOrCreateAsync<HackerNews>(storyId,
                async cacheEntry =>
                {
                    HackerNews? story = new HackerNews();

                    var response = await _hackerRepository.GetStoryByIdAsync(storyId);
                    if (response.IsSuccessStatusCode)
                    {
                        var storyResponse = response.Content.ReadAsStringAsync().Result;
                        story = JsonConvert.DeserializeObject<HackerNews>(storyResponse);
                    }

                    return story;
                });
        }
    }
}
