using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvaluator.Models
{
    internal class User
    {
        public int? UserId { get; }
        public string Username { get; }
        public string Password { get; set; }
        public string Code { get; set; }
        public int RoleId { get; set; }
        public string FolderLocation { get; }

        public User(string username, string password, string code, int roleId, string folderLocation = "", int? userId = null)
        {
            if (userId != null)
            {
                UserId = userId;
            }
            if (!string.IsNullOrEmpty(folderLocation))
            {
                FolderLocation = folderLocation;
            }
            Username = username;
            Password = password;
            Code = code;
            RoleId = roleId;
        }
    }
}
