using System;
using System.ComponentModel.DataAnnotations;

namespace AppMvc6.Models
{
    public enum Activity
    {
        [Display(Name = "年１～２回")]
        Year,
        [Display(Name = "半年１～２回")]
        TwoMonth,
        [Display(Name = "月１～２回")]
        Month,
        [Display(Name = "月３～４回")]
        TwoWeek,
        [Display(Name = "週１回")]
        Week,
        [Display(Name = "週１～２回")]
        Day,
        [Display(Name = "週２回以上")]
        More,
        [Display(Name = "不定期")]
        Other
    }
    public static class ActivityExtention
    {
        public static string Name(this Activity actibity)
        {
            if (!Enum.IsDefined(typeof(Activity), actibity))
            {
                return "";
            }

            string[] names = {
                "年１～２回",
                "半年１～２回",
                "月１～２回",
                "月３～４回",
                "週１回",
                "週１～２回",
                "週２回以上",
                "不定期"
            };
            return names[(int)actibity];
        }
    }
}