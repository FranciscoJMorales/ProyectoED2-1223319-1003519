using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Text;
using System.Text.Json;
using Processors;

namespace ProyectoED2.Controllers
{
    public class ChatController : Controller
    {
        private static readonly HttpClient client;
        private static User currentUser;
        private static User currentChat;

        static ChatController()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:57389/")
            };
        }

        // GET: ChatController
        public ActionResult Index()
        {
            currentUser = null;
            currentChat = null;
            return View();
        }

        [HttpPost]
        public ActionResult Index(IFormCollection collection)
        {
            User user = new User(collection["name"], collection["password"]);
            var json = new JsonResult(user);
            var response = client.PostAsync("login", new StringContent(json.ToString(), Encoding.UTF8, "application/json"));
            if (response.Result.IsSuccessStatusCode)
            {
                currentUser = user;
                return RedirectToAction("Users");
            }
            else
                return View();
        }

        public ActionResult SignIn()
        {
            ViewBag.IncorrectPassword = "";
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(IFormCollection collection)
        {
            if (collection["password"] == collection["password2"])
            {
                User user = new User(collection["name"], collection["password"]);
                var json = new JsonResult(user);
                var response = client.PostAsync("signin", new StringContent(json.ToString(), Encoding.UTF8, "application/json"));
                if (response.Result.IsSuccessStatusCode)
                {
                    currentUser = user;
                    return RedirectToAction("Users");
                }
                else
                    return View();
            }
            else
            {
                ViewBag.IncorrectPassword = "Las contraseñas no coinciden";
                return View();
            }
        }

        public ActionResult Users()
        {
            currentChat = null;
            var response = client.GetAsync("users");
            var text = response.Result.Content.ReadAsStringAsync();
            var lzw = new LZWCompressor();
            string content = lzw.ShowDecompress(text.Result);
            var list = JsonSerializer.Deserialize<List<UserView>>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            list.Remove(new UserView(currentUser));
            return View(list);
        }

        public ActionResult Chat()
        {
            var response = client.GetAsync($"chat/{currentUser.ID}/{currentChat.ID}");
            if (response.Result.IsSuccessStatusCode)
            {
                var text = response.Result.Content.ReadAsStringAsync();
                var lzw = new LZWCompressor();
                string content = lzw.ShowDecompress(text.Result);
                var list = JsonSerializer.Deserialize<List<MessageView>>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                return View(list);
            }
            else
                return RedirectToAction("Users");
        }

        [HttpPost]
        public ActionResult Chat(IFormCollection collection)
        {
            var message = new Message(currentUser, currentChat, collection["message"]);
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(IFormCollection collection)
        {
            return View();
        }

        public ActionResult OpenChat(string id)
        {
            var response = client.GetAsync($"user/{id}");
            if (response.Result.IsSuccessStatusCode)
            {
                var content = response.Result.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<User>(content.Result, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                currentChat = user;
                return RedirectToAction("Chat");
            }
            else
                return RedirectToAction("Users");
        }

        // GET: ChatController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ChatController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChatController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ChatController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ChatController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ChatController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ChatController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
