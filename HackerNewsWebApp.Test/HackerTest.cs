using HackerNewsWebApp.Core.Entities;
using HackerNewsWebApp.Core.Interfaces;
using HackerNewsWebApp.Core.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static System.Reflection.Metadata.BlobBuilder;

namespace HackerNewsWebApp.Test
{
    public class Tests
    {
        HackerNewsWebApp.Server.Controllers.HackerNewsController _hackerController;
        HackerNewsRepository _repository;
        IMemoryCache _cacheRepository;
        Logger<HackerNews> _logger;

        [SetUp]
        public void Setup()
        {
            _repository = new HackerNewsRepository();
            _hackerController = new Server.Controllers.HackerNewsController(_cacheRepository, _repository, _logger);
        }

        [Test]
        public void GetHackerNews()
        {
            var count = _hackerController.Get(null).Result.Count();
            Assert.That(200 == count);
        }
    }
}