using System;
using System.ComponentModel.DataAnnotations;

namespace AppMvc6.Models
{
    public enum Voice
    {
        [Display(Name = "混声")]
        Mixed,
        [Display(Name = "女声")]
        Female,
        [Display(Name = "男声")]
        Male,
        [Display(Name = "児童")]
        Child,
        [Display(Name = "その他")]
        Other
    }
    public static class VoiceExtention
    {
        public static string Name(this Voice voice)
        {
            if (!Enum.IsDefined(typeof(Voice), voice))
            {
                return "";
            }

            string[] names = {
                "混声",
                "女声",
                "男声",
                "児童",
                "その他"
            };
            return names[(int)voice];
        }
    }
}