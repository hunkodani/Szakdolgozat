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
    }
}
