using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppMvc6.ViewModels.Account
{
    public class ExternalLoginConfirmationViewModel
    {
        //[Required]
        //[EmailAddress]
        //public string Email { get; set; }

        [Required]
        [Display(Name = "ニックネーム")]
        public string ScreenName { get; set; }
    }
}
