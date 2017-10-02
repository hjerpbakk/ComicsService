using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hjerpbakk.ComicsService;
using Microsoft.AspNetCore.Mvc;

namespace Hjerpbakk.ComicService.Controllers
{
    [Route("api/[controller]")]
    public class ComicsController : Controller
    {
        readonly ComicsClient comicsClient;

        public ComicsController(ComicsClient comicsClient)
        {
            this.comicsClient = comicsClient;
        }

        // GET api/comics
        [HttpGet]
        public async Task<string> Get()
        {
            var imageURL = await comicsClient.GetNewestComicAsync();
            return imageURL;
        }
    }
}
