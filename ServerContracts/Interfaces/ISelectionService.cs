using ServerContracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServerContracts.Interfaces
{
    [ServiceContract]
    public interface ISelectionService
    {
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
    }
}
