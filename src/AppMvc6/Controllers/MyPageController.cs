using AppMvc6.Models;
using AppMvc6.ViewModels;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Extensions;
using Microsoft.AspNet.Http.Abstractions;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Net.Http.Headers;
using AppMvc6.Services;
using Microsoft.AspNet.Hosting;
//using ImageResizer;
using AppMvc6.Infrastructure;
using AutoMapper;

namespace AppMvc6.Controllers
{
    [Authorize]
    public class MyPageController : BaseController
    {
        [FromServices]
        private IHostingEnvironment hostingEnvironment { get; set; }
        [FromServices]
        private IGroupService groupService { get; set; }
        [FromServices]
        private IPracticeService practiceService { get; set; }
        [FromServices]
        private IProfileIconService profileIconService { get; set; }
        [FromServices]
        private IUserService userService { get; set; }

        public async Task<IActionResult> Index()
        {
            // todo マイページ
            var viewModel = new MyPageIndexViewModel();
            viewModel.Groups = await groupService.GetGroupsAsync(UserId);
            viewModel.PracticeViewModels = practiceService.GetTodayPracticeViewModels(viewModel.Groups);

            return View(viewModel);
        }

        public ActionResult Groups(int page = 1)
        {
            var groups = groupService.GetGroupsPage(new Page(page), UserId);

            return View();
        }

        public ActionResult Icon()
        {
            // todo アップロード画像のアクセス権限

            var user = User.ToApplicationUser();
            var icons = user.ProfileIcons;

            //Session["UserIconPath"] = user.IconPath;
            ViewBag.Message = TempData[nameof(UploadIcon)];
            return View(icons);
        }
        // [POST]画像アップロード
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadIcon(IFormFile uploadFile)
        {
            if (uploadFile == null)
            {
                return RedirectToAction(nameof(Icon));
            }

            var user = User.ToApplicationUser();
            if (user.ProfileIcons.Count < 5)
            {
                int size = 100; // MB
                string[] allowExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                string extension = System.IO.Path.GetExtension(uploadFile.GetFileName()).ToLower();
                if (!uploadFile.ContentType.StartsWith("image") || !allowExtentions.Contains(extension))
                {
                    TempData[nameof(UploadIcon)] = "拡張子が jpg, png, gif の画像ファイルのみ登録できます";
                    return RedirectToAction(nameof(Icon));
                }
                if (uploadFile.Length > size * 1024 * 1024)
                {
                    TempData[nameof(UploadIcon)] = "サイズ " + size + " MB までの画像ファイルのみ登録できます";
                    return RedirectToAction(nameof(Icon));
                }

                var icon = new ProfileIcon(user.Id, extension);

                //string physicalPath = Server.MapPath("~/" + icon.Path);
                string physicalPath = hostingEnvironment.WebRootPath + icon.Path;
                string directory = Path.GetDirectoryName(physicalPath);
                Directory.CreateDirectory(directory);

                //// ImageResizerによる画像変換＆ファイル保存
                //var imageJob = new ImageJob(uploadFile, physicalPath, new Instructions("maxwidth=360;maxheight=360;"));
                //imageJob.CreateParentDirectory = true;
                //imageJob.Build();

                uploadFile.SaveAs(physicalPath);
                if (System.IO.File.Exists(physicalPath))
                {
                    profileIconService.CreateIcon(icon);
                }
            }

            return RedirectToAction(nameof(Icon));
        }
        // [POST]画像選択
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectIcon(string iconPath)
        {
            //string physicalPath = Server.MapPath("~/" + iconPath);
            string physicalPath = hostingEnvironment.WebRootPath + iconPath;
            if (System.IO.File.Exists(physicalPath))
            {
                var user = userService.GetUser(UserId);
                user.IconPath = iconPath;
                userService.UpdateUser(user);

                //Session["UserIconPath"] = user.IconPath;
                TempData["Message"] = "アイコンを変更しました";
            }

            return RedirectToAction(nameof(Icon));
        }
        // [POST]画像削除
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteIcon(int iconId)
        {
            var icon = profileIconService.GetUserIcon(iconId, UserId);
            var user = User.ToApplicationUser();
            if (icon != null && icon.Path != user.IconPath)
            {
                profileIconService.DeleteIcon(icon);

                //string physicalPath = Server.MapPath("~/" + icon.Path);
                string physicalPath = hostingEnvironment.WebRootPath + icon.Path;
                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }
            }

            return RedirectToAction(nameof(Icon));
        }

        public ActionResult Withdraw()
        {
            return View();
        }
        [HttpPost, ActionName(nameof(Withdraw))]
        [ValidateAntiForgeryToken]
        public ActionResult WithdrawConfirmed()
        {
            userService.DeleteUser(UserId);

            // todo 退会時のアイコンファイル削除

            return RedirectToAction("GetLogOff", "Account");
        }

        public ActionResult Setting()
        {
            Mapper.CreateMap<ApplicationUser, MyPageSettingFormModel>();
            var formModel = Mapper.Map<MyPageSettingFormModel>(User.ToApplicationUser());

            return View(formModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Setting(MyPageSettingFormModel formModel)
        {
            if (!ModelState.IsValid)
            {
                return View(formModel);
            }

            Mapper.CreateMap<MyPageSettingFormModel, ApplicationUser>();
            var user = Mapper.Map(formModel, userService.GetUser(UserId));

            userService.UpdateUser(user);

            //Session["UserName"] = user.Name;
            TempData["Message"] = "プロフィールを変更しました";
            return RedirectToAction("Index", "Manage");
        }

        public ActionResult ChangeEmail()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeEmail(MyPageChangeEmailFormModel formModel)
        {
            if (!ModelState.IsValid)
            {
                return View(formModel);
            }

            if (!userService.CanUseEmail(UserId, formModel.Email))
            {
                TempData["Error"] = "そのメールアドレスは既に使用されています";
                return View(formModel);
            }

            var user = userService.GetUser(UserId);
            user.Email = formModel.Email;
            user.UserName = formModel.Email;

            userService.UpdateUser(user);

            TempData["Message"] = "メールアドレスを変更しました";
            return RedirectToAction("Index", "Manage");
        }
    }


    public static class FormFileExtension
    {
        public static string GetFileName(this IFormFile formFile)
        {
            using (var reader = new StreamReader(formFile.OpenReadStream()))
            {
                var fileContent = reader.ReadToEnd();
                var parsedContentDisposition = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition);
                return parsedContentDisposition.FileName;
            }
        }
    }
}
