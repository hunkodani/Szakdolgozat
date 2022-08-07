using AppEvaluator.Models;
using AppEvaluator.Stores;
using ServerContracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvaluator.NetworkingAndWCF
{
    internal static class WcfDataParser
    {
        public static bool LoginDataParser(ServerContracts.Models.User user)
        {
            if (user == null)
            {
                return false;
            }
            LoginDataStore.UserLoginData = new Models.User(user.Username, "", user.Code, user.RoleId, user.FolderLocation, user.UserId);
            return true;
        }

        public static List<Models.User> UsersParse(List<ServerContracts.Models.User> users)
        {
            if (users != null)
            {
                List<Models.User> result = new List<Models.User>();
                users.ForEach(user => result.Add(new Models.User(user.Username, user.Password, user.Code, user.RoleId, user.FolderLocation, user.UserId)));
                return result;
            }
            return null;
        }

        public static List<Models.Role> RolesParse(List<ServerContracts.Models.Role> roles)
        {
            if (roles != null)
            {
                List<Models.Role> result = new List<Models.Role>();
                roles.ForEach(role => result.Add(new Models.Role(role.RoleId, role.RoleName)));
                return result;
            }
            return null;
        }

        internal static List<Models.Subject> SubjectsParse(List<ServerContracts.Models.Subject> subjects)
        {
            if (subjects != null)
            {
                List<Models.Subject> result = new List<Models.Subject>();
                subjects.ForEach(subject => result.Add(new Models.Subject(subject.SubjectCode, subject.SubjectName, subject.FolderLocation)));
                return result;
            }
            return null;
        }

        internal static List<Models.Test> TestsParse(List<ServerContracts.Models.Test> tests)
        {
            if (tests != null)
            {
                List<Models.Test> result = new List<Models.Test>();
                tests.ForEach(test => result.Add(new Models.Test(test.TestId, test.TestName, test.SubjectCode)));
                return result;
            }
            return null;
        }
    }
}
