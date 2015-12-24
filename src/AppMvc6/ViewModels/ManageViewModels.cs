using Microsoft.AspNet.Identity;

namespace AppMvc6.ViewModels
{
    public class ManageIndexViewModel
    {
        public string Email { get; set; }
        public bool HasPassword { get; set; }

        public string ScreenName { get; set; }
        public string IconPath { get; set; }
        public string Note { get; set; }

        public UserLoginInfo LoginFacebook { get; set; }
        public UserLoginInfo LoginTwitter { get; set; }
        public bool CanRemove { get; set; }
    }
}