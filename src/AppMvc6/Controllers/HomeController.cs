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

        public IActionResult About()
        {
            var users = groupService.GetUsers();
            ViewBag.user = users.Result;
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            groupService.Update();
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
