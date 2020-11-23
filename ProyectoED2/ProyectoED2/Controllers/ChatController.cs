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
        public async Task<ActionResult> Index(IFormCollection collection)
        {
            User user = new User(collection["name"], collection["password"]);
            var response = await client.PostAsync("login", new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                user = JsonSerializer.Deserialize<User>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
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
        public async Task<ActionResult> SignIn(IFormCollection collection)
        {
            if (collection["password"] == collection["password2"])
            {
                User user = new User(collection["name"], collection["password"]);
                var response = await client.PostAsync("signin", new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    currentUser = user;
                    return RedirectToAction("Users");
                }
                else
                {
                    ViewBag.IncorrectPassword = "El usuario no es valido. Cambiar el nombre o la contraseña";
                    return View();
                }
            }
            else
            {
                ViewBag.IncorrectPassword = "Las contraseñas no coinciden";
                return View();
            }
        }

        public async Task<ActionResult> Users()
        {
            currentChat = null;
            var response = await client.GetAsync("users");
            if (response.IsSuccessStatusCode)
            {
                var text = await response.Content.ReadAsStringAsync();
                var lzw = new LZWCompressor();
                string content = lzw.ShowDecompress(text);
                var list = JsonSerializer.Deserialize<List<UserView>>(content);
                var user = new UserView(currentUser);
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].CompareTo(user) == 0)
                        list.RemoveAt(i);
                }
                return View(list);
            }
            else
                return RedirectToAction("Index");
        }

        public async Task<ActionResult> Chat()
        {
            var response = await client.GetAsync($"chat/{currentUser.ID}/{currentChat.ID}");
            if (response.IsSuccessStatusCode)
            {
                var text = await response.Content.ReadAsStringAsync();
                var lzw = new LZWCompressor();
                string content = lzw.ShowDecompress(text);
                var list = JsonSerializer.Deserialize<List<MessageView>>(content);
                return View(list);
            }
            else
                return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<ActionResult> Chat(IFormCollection collection)
        {
            var message = new Message(currentUser, currentChat, collection["message"]);
            await client.PostAsync("message", new StringContent(JsonSerializer.Serialize(message), Encoding.UTF8, "application/json"));
            return RedirectToAction("Chat");
        }

        public ActionResult Search()
        {
            ViewBag.Word = "";
            ViewBag.Results = 0;
            return View(new List<MessageView>());
        }

        [HttpPost]
        public async Task<ActionResult> Search(IFormCollection collection)
        {
            var response = await client.GetAsync($"search/{currentUser.ID}/{currentChat.ID}/{collection["text"]}");
            if (response.IsSuccessStatusCode)
            {
                var text = await response.Content.ReadAsStringAsync();
                var lzw = new LZWCompressor();
                string content = lzw.ShowDecompress(text);
                var list = JsonSerializer.Deserialize<List<MessageView>>(content);
                ViewBag.Word = collection["text"];
                ViewBag.Results = list.Count;
                return View(list);
            }
            else
                return RedirectToAction("Chat");
        }

        public async Task<ActionResult> OpenChat(string id)
        {
            var response = await client.GetAsync($"user/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<User>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                currentChat = user;
                return RedirectToAction("Chat");
            }
            else
                return RedirectToAction("Users");
        }
    }
}
