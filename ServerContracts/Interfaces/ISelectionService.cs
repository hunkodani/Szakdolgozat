using ServerContracts.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace ServerContracts.Interfaces
{
    [ServiceContract]
    public interface ISelectionService
    {
        [OperationContract]
        bool ConnectionTest();

        [OperationContract]
        User Login(string name, string password);

        [OperationContract]
        string GetRoleName(int roleId);

        [OperationContract]
        List<Subject> GetSubjects();

        [OperationContract]
        List<User> GetUsers();

        [OperationContract]
        List<Role> GetRoles();

        [OperationContract]
        List<Test> GetTests(string subjectCode);

        [OperationContract]
        List<Test> GetUserAvailableTest(string subjectCode, int userId);

        [OperationContract]
        List<User> GetUsersOnTest(int testId);

        [OperationContract]
        bool SaveNewPassword(int userId, string newPass);


        #region DeleteFunctions

        [OperationContract]
        void DeleteAssignment(int userId, int testId);

        [OperationContract]
        void DeleteTest(int testId, string testLocation);

        [OperationContract]
        void DeleteSubject(string code, string subjectLocation);

        [OperationContract]
        void DeleteUser(int userId, string userLocaiton);

        #endregion

        #region InsertFunctions

        [OperationContract]
        void InsertAssignment(int userId, int testId);

        #endregion

        #region UpdateFunctions

        [OperationContract]
        void UpdateUser(int userId, string pass = null, string code = null);

        #endregion

    }
}
