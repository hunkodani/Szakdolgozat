using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvaluatorServer
{
    internal class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
        public int RoleId { get; set; }
        public string FolderLocation { get; set; }

        public User(int userId, string username, string password, string code, int roleId, string folderLocation)
        {
            UserId = userId;
            Username = username;
            Password = password;
            Code = code;
            RoleId = roleId;
            FolderLocation = folderLocation;
        }
    }
}
