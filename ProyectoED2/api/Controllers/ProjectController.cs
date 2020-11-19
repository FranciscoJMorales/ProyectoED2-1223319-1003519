using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        readonly IWebHostEnvironment env;

        public ProjectController(IWebHostEnvironment _env)
        {
            env = _env;
        }

        [HttpGet]
        public IActionResult Default()
        {

            string path = env.EnvironmentName;
            return NoContent();
        }

        [HttpPost]
        [Route("signin")]
        public IActionResult SignIn()
        {
            return NoContent();
        }

        [HttpPost]
        public IActionResult Example()
        {
            return NoContent();
        }
    }
}
