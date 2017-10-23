using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Hjerpbakk.ComicsService.Controllers
{
    [Route("api/[controller]")]
    public class VersionController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}