using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AppEvaluator.Models
{
    [DataContract]
    internal class User
    {
        [DataMember]
        public int? UserId { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public int RoleId { get; set; }
        [DataMember]
        public string FolderLocation { get; set; }

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
