using HackerNewsWebApp.Core.Entities;
using HackerNewsWebApp.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNewsWebApp.Core.Repository
{
    public class HackerNewsRepository : IHackerNewsRepository
    {
        private IMemoryCache _cacheRepository;

        private static HttpClient client = new HttpClient();

        public HackerNewsRepository(IMemoryCache cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }

        public async Task<List<HackerNews>> GetFilteredStory(string? searchTerm)
        {
            List<HackerNews> stories = new List<HackerNews>();

            var response = await client.GetAsync("https://hacker-news.firebaseio.com/v0/beststories.json");
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
        public async Task<HackerNews?> GetStoryAsync(int storyId)
        {
            return await _cacheRepository.GetOrCreateAsync<HackerNews>(storyId,
                async cacheEntry =>
                {
                    HackerNews? story = new HackerNews();

                    var response = await client.GetAsync(string.Format("https://hacker-news.firebaseio.com/v0/item/{0}.json", storyId));
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
