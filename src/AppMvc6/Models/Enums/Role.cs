using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppMvc6.Models
{
    public enum Role
    {
        None = 0,
        Manager = 1,
        Owner = 2,
    }
    public static class RoleExtention
    {
        public static string Name(this Role role)
        {
            if (!Enum.IsDefined(typeof(Role), role))
            {
                return "";
            }

            string[] names = {
                "一般",
                "副管理人",
                "管理人"
            };
            return names[(int)role];
        }
    }
}
