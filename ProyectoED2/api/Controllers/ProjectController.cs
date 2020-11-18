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
            var client = new MongoClient("mongodb+srv://FranciscoJMorales:ProyectoED2@projectediicluster.uuw4c.mongodb.net/ChatDB?retryWrites=true&w=majority");
            var database = client.GetDatabase("ChatDB");
            var collection = database.GetCollection<BsonDocument>("Messages");
            collection.Find(new BsonDocument()).ToList();
            return NoContent();
        }
    }
}
