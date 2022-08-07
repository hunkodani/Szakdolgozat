using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServerContracts.Models
{
    [DataContract]
    public class User
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

        public User(string username, string password, string code, int roleId, string folderLocation, int? userId)
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
