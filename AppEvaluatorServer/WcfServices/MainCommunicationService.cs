using ServerContracts.Interfaces;
using ServerContracts.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
        public bool ConnectionTest()
        {
            return true;
        }
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

        public List<Test> GetUserAvailableTest(string subjectCode, int userId)
        {
            return SQLiteMethods.GetUserAvailableTests(subjectCode, userId);
        }

        public List<User> GetUsersOnTest(int testId)
        {
            return SQLiteMethods.GetUsersOnTest(testId);
        }

        public bool SaveNewPassword(int userId, string newPass)
        {
            return SQLiteMethods.UpdatePassword(userId, newPass);
        }


        #region DeleteFunctions

        public void DeleteAssignment(int userId, int testId)
        {
            SQLiteMethods.DeleteAssignment(userId, testId);
        }

        public void DeleteTest(int testId, string testLocation)
        {
            SQLiteMethods.DeleteTestAssignments(testId);
            SQLiteMethods.DeleteTest(testId);
            //Delete files from disk --> from root/testLocation
            if (Directory.Exists(Path.Combine(FileMethods.DataRoot, "Subjects", testLocation)))
            {
                Directory.Delete(Path.Combine(FileMethods.DataRoot, "Subjects", testLocation), true);
            }
        }

        public void DeleteSubject(string code, string subjectLocation)
        {
            List<Test> tests = SQLiteMethods.GetTests(code);
            tests.ForEach(item => {
                SQLiteMethods.DeleteTestAssignments(item.TestId);
            });
            tests.Clear();
            SQLiteMethods.DeleteSubjectTests(code);
            SQLiteMethods.DeleteSubject(code);
            //Delete files from disk --> from root/subjectLocation
            if (Directory.Exists(Path.Combine(FileMethods.DataRoot, subjectLocation)))
            {
                Directory.Delete(Path.Combine(FileMethods.DataRoot, subjectLocation), true);
            }
        }

        public void DeleteUser(int userId, string userLocaiton)
        {
            SQLiteMethods.DeleteUserAssignments(userId);
            SQLiteMethods.DeleteUser(userId);
            //Delete files from disk --> from root/userLocation
            if (Directory.Exists(Path.Combine(FileMethods.DataRoot, userLocaiton)))
            {
                Directory.Delete(Path.Combine(FileMethods.DataRoot, userLocaiton), true);
            }
        }

        #endregion

        #region InsertFunctions

        public void InsertAssignment(int userId, int testId)
        {
            SQLiteMethods.InsertAssignment(userId, testId);
        }

        #endregion

    }

}
