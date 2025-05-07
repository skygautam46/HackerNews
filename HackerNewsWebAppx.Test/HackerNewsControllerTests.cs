using HackerNewsWebApp.Core.Entities;
using HackerNewsWebApp.Core.Interfaces;
using HackerNewsWebApp.Server.Controllers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace HackerNewsWebAppx.Test
{
    public class HackerNewsControllerTests
    {
        private readonly Mock<IHackerNewsRepository> _hackerRepositoryMock;
        IMemoryCache _cacheRepository;
        Logger<HackerNews> _logger;

        public HackerNewsControllerTests()
        {
            _hackerRepositoryMock = new Mock<IHackerNewsRepository>();
        }

        [Fact]
        public async void Get_HackerNews_WhenFilteredNewsMatched()
        {
            // Arrange

            var expectedNews = new HackerNews { Title = "Metagenomics test saves woman's sight after mystery infection", By = "neversaydie", Url = "https://www.bbc.co.uk/news/articles/czx45vze0vyo" };
            List<HackerNews> news = new List<HackerNews> { expectedNews };

            _hackerRepositoryMock.Setup(x => x.GetFilteredStory("test")).ReturnsAsync(news);

            var controller = new HackerNewsController(_cacheRepository, _hackerRepositoryMock.Object, _logger);

            // Act
            var result = await controller.Get("test");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedNews, result.FirstOrDefault());
        }

        [Fact]
        public async void Get_HackerNews_When_NotNull()
        {
            // Arrange

            var expectedUser = new HackerNews { Title = "Metagenomics test saves", By = "gautam", Url = "https://www.bbc.co.uk/news/articles/czx45vze0vyo" };
            List<HackerNews> news = new List<HackerNews> { expectedUser };

            _hackerRepositoryMock.Setup(x => x.GetFilteredStory("test")).ReturnsAsync(news);

            var controller = new HackerNewsController(_cacheRepository, _hackerRepositoryMock.Object, _logger);

            // Act
            var result = await controller.Get("test");

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Test_Count_Fails_When_Expected_Is_Incorrect()
        {
            var expectedUser = new HackerNews { Title = "Metagenomics test saves", By = "gautam", Url = "https://www.bbc.co.uk/news/articles/czx45vze0vyo" };
            List<HackerNews> news = new List<HackerNews> { expectedUser };

            _hackerRepositoryMock.Setup(x => x.GetFilteredStory("test")).ReturnsAsync(news);

            var controller = new HackerNewsController(_cacheRepository, _hackerRepositoryMock.Object, _logger);

            // Act
            var result = await controller.Get("test");

            // Assert
            Assert.NotEqual(5, result.Count());
        }
    }
}