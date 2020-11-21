using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Processors;
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
            user = MongoDBManager.FindUser(user.Name, user.Password);
            if (user != null)
                return Ok(user);
            else
                return NotFound();
        }

        [HttpPost]
        [Route("/signin")]
        public IActionResult SignIn(User user)
        {
            if (MongoDBManager.AddUser(user))
                return StatusCode(201);
            else
                return Conflict();
        }

        [HttpGet]
        [Route("/users")]
        public IActionResult Users()
        {
            try
            {
                var list = MongoDBManager.UserViews();
                string json = JsonSerializer.Serialize(list);
                var lzw = new LZWCompressor();
                return Ok(lzw.ShowCompress(json));
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("/chat/{IdEmisor}/{IdReceptor}")]
        public IActionResult Chat(string IdEmisor, string IdReceptor)
        {
            try
            {
                var list = MongoDBManager.ChatView(IdEmisor, IdReceptor);
                string json = JsonSerializer.Serialize(list);
                var lzw = new LZWCompressor();
                return Ok(lzw.ShowCompress(json));
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("/message")]
        public IActionResult Message(Message message)
        {
            MongoDBManager.AddMessage(message);
            return Ok();
        }

        [HttpGet]
        [Route("/search/{IdEmisor}/{IdReceptor}/{text}")]
        public IActionResult Search(string IdEmisor, string IdReceptor, string text)
        {
            try
            {
                var list = MongoDBManager.Search(IdEmisor, IdReceptor, text);
                string json = JsonSerializer.Serialize(list);
                var lzw = new LZWCompressor();
                return Ok(lzw.ShowCompress(json));
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("/user/{id}")]
        public IActionResult GetUser(string id)
        {
            var user = MongoDBManager.FindUser(id);
            if (user != null)
                return Ok(user);
            else
                return NotFound();
        }
    }
}
