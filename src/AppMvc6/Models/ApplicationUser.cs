using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Principal;
using System.Security.Claims;
using Microsoft.AspNet.Http.Internal;
using Microsoft.Data.Entity;

namespace AppMvc6.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<int>
    {
        public string ScreenName { get; set; }
        public string IconPath { get; set; }
        public string Note { get; set; }
    }

    public static class ApplicationUserExtention
    {
        /// <summary>
        /// ClaimsPrincipalからApplicationUserを取得するための拡張メソッド
        /// </summary>
        public static ApplicationUser ToApplicationUser(this ClaimsPrincipal user)
        {
            int userId = int.Parse(user.GetUserId());
            return UserManager().Users.FirstOrDefault(u => u.Id == userId);
        }
        /// <summary>
        /// ClaimsPrincipalから非同期でApplicationUserを取得するための拡張メソッド
        /// </summary>
        public static async Task<ApplicationUser> ToApplicationUserAsync(this ClaimsPrincipal user)
        {
            int userId = int.Parse(user.GetUserId());
            return await UserManager().Users.FirstOrDefaultAsync(u => u.Id == userId);
        }


        private static UserManager<ApplicationUser> UserManager()
        {
            return (UserManager<ApplicationUser>)new HttpContextAccessor().HttpContext.ApplicationServices.GetService(typeof(UserManager<ApplicationUser>));
        }
    }
}
