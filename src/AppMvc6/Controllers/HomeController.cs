using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AppMvc6.Models;
using AppMvc6.Services;
using AppMvc6.Infrastructure;
using System.Security.Claims;

namespace AppMvc6.Controllers
{
    public class HomeController : BaseController
    {
        [FromServices]
        public IGroupService groupService { get; set; }


        public async Task<IActionResult> Index(int page = 1)
        {
            Page pages = new Page(page, 10);
            var groups = await groupService.GetGroupsPageAsync(pages);

            return View(groups);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enquete(string content, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // todo Enquete実装
                //var enquete = new Enquete() { UserName = User.Identity.Name, Content = content };
                //enqueteService.CreateEnquete(enquete);
                await Task.Delay(3000);

                TempData["Message"] = "ご意見ありがとうございます。これからも Chorus Studio をよろしくお願いします。";
            }

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        //public IActionResult About()
        //{
        //    var user = GetCurrentUser();
        //    return View();
        //}
        public async Task<IActionResult> About()
        {
            var user = await GetCurrentUserAsync();
            return View();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[ValidateInput(false)]
        //public ActionResult About(TestViewModel viewModel)
        //{
        //    var options = new MarkdownSharp.MarkdownOptions()
        //    {
        //        EncodeProblemUrlCharacters = true
        //    };
        //    var markdown = new MarkdownSharp.Markdown(options);
        //    string encodeText = HttpUtility.HtmlEncode(viewModel.Content);
        //    string html = markdown.Transform(encodeText);

        //    //viewModel.Content = html;
        //    return View(viewModel);
        //}

        public IActionResult Test()
        {
            return View();
        }


        
        public IActionResult Error()
        {
            return View();
        }
    }
}
