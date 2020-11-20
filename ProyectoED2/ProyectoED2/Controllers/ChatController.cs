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
            return View();
        }

        [HttpPost]
        public ActionResult Index(IFormCollection collection)
        {
            User user = new User(collection["name"], collection["password"]);
            var json = new JsonResult(new User(collection[""], collection[""]));
            var response = client.PostAsync("login", new StringContent(json.ToString(), Encoding.UTF8, "application/json"));
            if (response.Result.IsSuccessStatusCode)
            {
                currentUser = user;
                return RedirectToAction("Users", user);
            }
            else
                return View();
        }

        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(IFormCollection collection)
        {
            if (collection["password"] == collection["password2"])
            {
                User user = new User(collection["name"], collection["password"]);
                var json = new JsonResult(new User(collection["name"], collection["password"]));
                var response = client.PostAsync("signin", new StringContent(json.ToString(), Encoding.UTF8, "application/json"));
                if (response.Result.IsSuccessStatusCode)
                {
                    currentUser = user;
                    return RedirectToAction("Users", user);
                }
                else
                    return View();
            }
            else
            {
                ViewBag.IncorrectPassword = true;
                return View();
            }
        }

        public ActionResult Users()
        {
            var response = client.GetAsync("users");
            var content = response.Result.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<Message>>(content.Result, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            return View(list);
        }

        public ActionResult Chat(string receptor)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Chat(IFormCollection collection)
        {
            return View();
        }

        public ActionResult OpenChat(string id)
        {
            return RedirectToAction("Chat");
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
