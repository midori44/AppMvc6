using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AppMvc6.Models;
using AppMvc6.Services;
using AppMvc6.Infrastructure;
using System.Security.Claims;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Http.Internal;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Http.Features;

namespace AppMvc6.Controllers
{
    /// <summary>
    /// ApplicationUser取得用メソッドを追加した抽象クラス
    /// </summary>
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// 現在のユーザーIDをint型で取得
        /// </summary>
        protected int UserId => int.Parse(User.GetUserId());

        /// <summary>
        /// 現在のユーザーをApplicationUser型で取得
        /// </summary>
        /// <returns></returns>
        protected ApplicationUser GetCurrentUser()
        {
            var user = User.ToApplicationUser();
            SetSession(user);
            return user;
        }

        /// <summary>
        /// 現在のユーザーをApplicationUser型で非同期に取得
        /// </summary>
        /// <returns></returns>
        protected async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var user = await User.ToApplicationUserAsync();
            SetSession(user);
            return user;
        }

        /// <summary>
        /// ユーザー情報をセッションに保存
        /// </summary>
        /// <param name="user"></param>
        protected void SetSession(ApplicationUser user)
        {
            if (user != null)
            {
                var session = new MySession(HttpContext);
                session["UserScreenName"] = user.ScreenName;
                session["UserIconPath"] = user.IconPath;
            }
        }
    }
}
