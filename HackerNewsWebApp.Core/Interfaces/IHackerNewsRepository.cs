using HackerNewsWebApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNewsWebApp.Core.Interfaces
{
    public interface IHackerNewsRepository
    {
        Task<List<HackerNews>> GetFilteredStory(string? searchTerm);
        Task<HackerNews?> GetStoryAsync(int storyId);
    }
}
