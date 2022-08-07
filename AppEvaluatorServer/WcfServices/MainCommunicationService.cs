using ServerContracts.Interfaces;
using ServerContracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AppEvaluatorServer.WcfServices
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MainCommunicationService : ISelectionService
    {
        public User Login(string name, string password)
        {
            return SQLiteMethods.GetLoginData(name, password);
        }

        public string GetRoleName(int roleId) 
        { 
            return SQLiteMethods.GetRoleName(roleId); 
        }

        public List<Subject> GetSubjects()
        {
            return SQLiteMethods.GetSubjects();
        }

        public List<User> GetUsers()
        {
            return SQLiteMethods.GetUsers();
        }

        public List<Role> GetRoles()
        {
            return SQLiteMethods.GetRoles();
        }

        public List<Test> GetTests(string subjectCode)
        {
            return SQLiteMethods.GetTests(subjectCode);
        }
    }

}
