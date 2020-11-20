using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        [HttpPost]
        [Route("/login")]
        public IActionResult LogIn(User user)
        {
            return Ok();
        }

        [HttpPost]
        [Route("/signin")]
        public IActionResult SignIn(User user)
        {
            return Ok();
        }

        [HttpGet]
        [Route("/users")]
        public IActionResult Users()
        {
            return Ok();
        }

        [HttpGet]
        [Route("/chat/{IdEmisor}/{IdReceptor}")]
        public IActionResult Chat(string IdEmisor, string IdReceptor)
        {
            return Ok();
        }

        [HttpPost]
        [Route("/message")]
        public IActionResult Message(Message message)
        {
            return Ok();
        }

        [HttpGet]
        [Route("/search/{IdEmisor}/{IdReceptor}/{text}")]
        public IActionResult Search(string IdEmisor, string IdReceptor, string text)
        {
            return Ok();
        }

        [HttpGet]
        [Route("/user/{id}")]
        public IActionResult GetUser(string id)
        {
            return Ok();
        }
    }
}
