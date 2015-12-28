using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppMvc6.Models
{
    public enum SongProgress
    {
        [Display(Name = "練習中")]
        Going,
        [Display(Name = "保留")]
        Pending,
        [Display(Name = "終了")]
        Closed
    }
    public static class SongProgressExtention
    {
        public static string Name(this SongProgress progress)
        {
            if (!Enum.IsDefined(typeof(SongProgress), progress))
            {
                return "";
            }

            string[] names = {
                "練習中",
                "保留",
                "終了"
            };
            return names[(int)progress];
        }
    }
}
