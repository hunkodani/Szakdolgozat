using AppEvaluator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvaluator.Stores
{
    internal static class LoginDataStore
    {
        public static User UserLoginData { get; set; }
        public static string RoleName { get; set; }

        public static void ClearLoginData()
        {
            UserLoginData = null;
            RoleName = null;
        }
    }
}
