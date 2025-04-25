using HackerNewsWebApp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNewsWebApp.Core.Repository
{
    public class HackerNewsRepository : IHackerNewsRepository
    {

        private static HttpClient client = new HttpClient();

        public HackerNewsRepository()
        {

        }

        public async Task<HttpResponseMessage> BestStoriesAsync()
        {
            return await client.GetAsync("https://hacker-news.firebaseio.com/v0/beststories.json");
        }

        public async Task<HttpResponseMessage> GetStoryByIdAsync(int id)
        {
            return await client.GetAsync(string.Format("https://hacker-news.firebaseio.com/v0/item/{0}.json", id));
        }
    }
}
