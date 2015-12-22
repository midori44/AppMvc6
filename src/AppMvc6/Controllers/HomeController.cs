using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AppMvc6.Models;
using AppMvc6.Services;

namespace AppMvc6.Controllers
{
    public class HomeController : Controller
    {
        [FromServices]
        public IGroupService groupService { get; set; }
        
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {
            var users = await groupService.GetUsers();
            //ViewBag.user = users.Result;
            foreach(var user in users)
            {
                Console.WriteLine(user.UserName);
            }

            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public async Task<IActionResult> Contact()
        {
            await groupService.Update();
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
