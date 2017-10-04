using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hjerpbakk.ComicsService.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Hjerpbakk.ComicService.Controllers
{
    [Route("api/[controller]")]
    public class ComicsController : Controller
    {
        readonly ComicsClient comicsClient;
        readonly IMemoryCache memoryCache;

        public ComicsController(ComicsClient comicsClient, IMemoryCache memoryCache)
        {
            this.comicsClient = comicsClient;
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            var imageURL = await comicsClient.GetNewestComicAsync();
            return imageURL;
        }
    }
}
