using AppMvc6.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppMvc6.ViewModels
{
    public class MyPageIndexViewModel
    {
        public IEnumerable<Group> Groups { get; set; }
        public IEnumerable<PracticeViewModel> PracticeViewModels { get; set; }
    }

    public class MyPageSettingFormModel
    {
        [Required]
        [MaxLength(32)]
        [Display(Name = "ニックネーム")]
        public string Name { get; set; }
        [MaxLength(400)]
        [Display(Name = "自己紹介")]
        public string Note { get; set; }
    }

    public class MyPageChangeEmailFormModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "メールアドレス")]
        public string Email { get; set; }
    }
}
