using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using ServerContracts.Models;
using System.IO;
using ServerContracts;

namespace AppEvaluatorServer
{
    internal static class SQLiteMethods
    {
        private static SQLiteConnection _conn;
        public static string Database { get; set; }
        public static bool ConnectionStatus { get; private set; }

        /// <summary>
        /// Connects to the specified database and verifies the tables
        /// </summary>
        /// <returns>Returns true if successfully connected to the database, false otherwise</returns>
        public static void ConnectToDatabase()
        {
            Database = "database.db";

            string sqlConnString = "Data Source=" + Database + ";Version=3;Compress=True;UTF8Encoding=True;";
            _conn = new SQLiteConnection(sqlConnString);
            try
            {
                _conn.Open();
                VerifyTableExistence();
                ConnectionStatus = true;
            }
            catch (Exception ex)
            {
                Logging.WriteToLog(LogTypes.Error, ex.Message);
                ConnectionStatus = false;
            }
        }

        /// <summary>
        /// Disconnects from the database
        /// </summary>
        public static void DisconnectFromDatabase()
        {
            if (_conn != null && _conn.State == System.Data.ConnectionState.Open)
            {
                _conn.Close();
                ConnectionStatus = false;
            }
        }

        /// <summary>
        /// Verifies the existence of the tables in the database, if missing any, creates it
        /// </summary>
        private static void VerifyTableExistence()
        {
            SQLiteDataReader dataReader;
            SQLiteCommand cmd = _conn.CreateCommand();
            List<string> list = new List<string>();

            cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table'";
            dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                list.Add(dataReader.GetString(0));
            }
            dataReader.Close();
            if (!list.Contains("Roles"))
            {
                cmd.CommandText = "Create Table Roles (RoleId INTEGER Primary key, RoleName Varchar(50) Not null)";
                _ = cmd.ExecuteNonQuery();
                cmd.CommandText = "Insert Into Roles Values(null, \"user\")";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "Insert Into Roles Values(null, \"teacher\")";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "Insert Into Roles Values(null, \"admin\")";
                cmd.ExecuteNonQuery();
            }
            if (!list.Contains("Users"))
            {
                cmd.CommandText = "Create Table Users (UserId INTEGER Primary key, " +
                                                        "Username Varchar(100) Not null, " +
                                                        "Password Varchar(255) Not null, " +
                                                        "Code Varchar(10), " +
                                                        "RoleId INTEGER Not null, " +
                                                        "FolderLocation Varchar(255)," +
                                                        "UNIQUE(Username, Code)," +
                                                        "FOREIGN KEY(RoleId) REFERENCES Roles(RoleId))";
                _ = cmd.ExecuteNonQuery();
                string pass = EncrypterDecrypterService.Encrypt("admin", EncrypterDecrypterService.Key);
                string username = "admin";
                cmd.CommandText = "Insert Into Users (UserId, Username, Password, Code, RoleId, FolderLocation) Values (null, @username, @pass, @code, 3, @folder)";
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("pass", pass);
                cmd.Parameters.AddWithValue("code", "");
                cmd.Parameters.AddWithValue("folder", Path.Combine("Users", username));
                _ = cmd.ExecuteNonQuery();
            }
            if (!list.Contains("Tests"))
            {
                cmd.CommandText = "Create Table Tests (TestId INTEGER Primary key, TestName Varchar(100), " +
                                    "SubjectCode Varchar(50), UNIQUE(TestName), FOREIGN KEY(SubjectCode) REFERENCES Subjects(SubjectCode))";
                _ = cmd.ExecuteNonQuery();
            }
            if (!list.Contains("Subjects"))
            {
                cmd.CommandText = "Create Table Subjects (SubjectCode Varchar(50) Primary key, SubjectName Varchar(150), FolderLocation Varchar(255), " +
                                    "UNIQUE(SubjectCode, SubjectName))";
                _ = cmd.ExecuteNonQuery();
            }
            if (!list.Contains("Assignments"))
            {
                cmd.CommandText = "Create Table Assignments (TestId INTEGER, UserId INTEGER, " +
                                    "FOREIGN KEY(TestId) REFERENCES Tests(TestId), FOREIGN KEY(UserId) REFERENCES Users(UserId))";
                _ = cmd.ExecuteNonQuery();
            }
        }

        #region Insert Methods

        internal static void InsertUser(string username, string pass, string code, int role)
        {
            SQLiteCommand cmd = null;
            try
            {
                cmd = _conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Users(UserId, Username, Password, Code, RoleId, FolderLocation) VALUES (null, @name, @pass, @code, @role, @folder)";
                cmd.Parameters.AddWithValue("name", username);
                cmd.Parameters.AddWithValue("pass", pass);
                cmd.Parameters.AddWithValue("code", code);
                cmd.Parameters.AddWithValue("role", role);
                cmd.Parameters.AddWithValue("folder", Path.Combine("Users", username));
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }
            cmd?.Dispose();
        }

        internal static void InsertSubject(string code, string name)
        {
            SQLiteCommand cmd = null;
            try
            {
                cmd = _conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Subjects(SubjectCode, SubjectName, FolderLocation) VALUES (@code, @name, @folder)";
                cmd.Parameters.AddWithValue("code", code);
                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("folder", Path.Combine("Subjects", code));
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }
            cmd?.Dispose();
        }

        internal static void InsertTest(string testName, string subjectCode)
        {
            SQLiteCommand cmd = null;
            try
            {
                cmd = _conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Tests(TestId, TestName, SubjectCode) VALUES (null, @name, @code)";
                cmd.Parameters.AddWithValue("name", testName);
                cmd.Parameters.AddWithValue("code", subjectCode);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }
            cmd?.Dispose();
        }

        internal static void InsertAssignment(int userId, int testId)
        {
            SQLiteCommand cmd = null;
            try
            {
                cmd = _conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Assignments(UserId, TestId) VALUES (@user, @test)";
                cmd.Parameters.AddWithValue("user", userId);
                cmd.Parameters.AddWithValue("test", testId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }
            cmd?.Dispose();
        }

        #endregion

        #region Update Methods

        internal static void UpdateUser(string username, string pass, string code, int role, int userId)
        {
            SQLiteCommand cmd = null;
            try
            {
                cmd = _conn.CreateCommand();
                cmd.CommandText = "Update Users SET Username = @name, Password = @pass, Code = @code, RoleId = @role WHERE UserId = @id";
                cmd.Parameters.AddWithValue("name", username);
                cmd.Parameters.AddWithValue("pass", pass);
                cmd.Parameters.AddWithValue("code", code);
                cmd.Parameters.AddWithValue("role", role);
                cmd.Parameters.AddWithValue("id", userId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }
            cmd?.Dispose();
        }

        internal static void UpdateSubject(string code, string name)
        {
            SQLiteCommand cmd = null;
            try
            {
                cmd = _conn.CreateCommand();
                cmd.CommandText = "Update Subjects SET SubjectName = @name WHERE SubjectCode = @code";
                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("code", code);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }
            cmd?.Dispose();
        }

        internal static void UpdateTest(string testName, string subjectCode, int testId)
        {
            SQLiteCommand cmd = null;
            try
            {
                cmd = _conn.CreateCommand();
                cmd.CommandText = "Update Tests SET TestName = @name, SubjectCode = @code WHERE TestId = @id";
                cmd.Parameters.AddWithValue("name", testName);
                cmd.Parameters.AddWithValue("code", subjectCode);
                cmd.Parameters.AddWithValue("id", testId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }
            cmd?.Dispose();
        }

        internal static bool UpdatePassword(int userId, string newPass)
        {
            SQLiteCommand cmd = null;
            try
            {
                cmd = _conn.CreateCommand();
                cmd.CommandText = "Update Users SET Password = @pass WHERE UserId = @id";
                cmd.Parameters.AddWithValue("id", userId);
                cmd.Parameters.AddWithValue("pass", newPass);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                cmd?.Dispose();
                return false;
            }
            cmd?.Dispose();
            return true;
        }

        #endregion

        #region Delete Methods

        internal static void DeleteUser(int userId)
        {
            SQLiteCommand cmd = null;
            try
            {
                cmd = _conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Users WHERE UserId = @id";
                cmd.Parameters.AddWithValue("id", userId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }
            cmd?.Dispose();
        }

        internal static void DeleteSubject(string code)
        {
            SQLiteCommand cmd = null;
            try
            {
                cmd = _conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Subjects WHERE SubjectCode = @code";
                cmd.Parameters.AddWithValue("code", code);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }
            cmd?.Dispose();
        }

        internal static void DeleteTest(int testId)
        {
            SQLiteCommand cmd = null;
            try
            {
                cmd = _conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Tests WHERE TestId = @id";
                cmd.Parameters.AddWithValue("id", testId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }
            cmd?.Dispose();
        }

        internal static void DeleteAssignments(int userID, int testId)
        {
            SQLiteCommand cmd = null;
            try
            {
                cmd = _conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Assignments WHERE UserId = @user AND TestId = @test";
                cmd.Parameters.AddWithValue("user", userID);
                cmd.Parameters.AddWithValue("test", testId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }
            cmd?.Dispose();
        }

        #endregion

        #region Read Methods

        internal static List<User> GetUsers()
        {
            List<User> list = new List<User>();
            SQLiteDataReader dataReader = null;
            SQLiteCommand cmd = null;
            try
            {
                cmd = _conn.CreateCommand();

                cmd.CommandText = "SELECT * FROM Users";
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    list.Add(new User(username: dataReader.GetString(1),
                                      password: "",
                                      code: dataReader.GetString(3),
                                      roleId: dataReader.GetInt32(4),
                                      folderLocation: dataReader.GetString(5),
                                      userId: dataReader.GetInt32(0)));
                }
                dataReader.Close();
            }
            catch (Exception)
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
                cmd?.Dispose();
            }
            return list;
        }

        internal static List<Role> GetRoles()
        {
            List<Role> list = new List<Role>();
            SQLiteDataReader dataReader = null;
            SQLiteCommand cmd = null;
            try
            {
                cmd = _conn.CreateCommand();

                cmd.CommandText = "SELECT * FROM Roles";
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    list.Add(new Role(dataReader.GetInt32(0),
                                        dataReader.GetString(1)));
                }
                dataReader.Close();
            }
            catch (Exception)
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
                cmd?.Dispose();
            }
            return list;
        }

        internal static List<Subject> GetSubjects()
        {
            List<Subject> list = new List<Subject>();
            SQLiteDataReader dataReader = null;
            SQLiteCommand cmd = null;
            try
            {
                cmd = _conn.CreateCommand();

                cmd.CommandText = "SELECT * FROM Subjects";
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    list.Add(new Subject(dataReader.GetString(0),
                                        dataReader.GetString(1),
                                        dataReader.GetString(2)));
                }
                dataReader.Close();
            }
            catch (Exception)
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
                cmd?.Dispose();
            }
            return list;
        }

        internal static List<Test> GetTests(string subjectCode)
        {
            List<Test> list = new List<Test>();
            SQLiteDataReader dataReader = null;
            SQLiteCommand cmd = null;
            try
            {
                cmd = _conn.CreateCommand();

                cmd.CommandText = "SELECT * FROM Tests WHERE SubjectCode = @code";
                cmd.Parameters.AddWithValue("code", subjectCode);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    list.Add(new Test(dataReader.GetInt32(0),
                                        dataReader.GetString(1),
                                        dataReader.GetString(2)));
                }
                dataReader.Close();
            }
            catch (Exception)
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
                cmd?.Dispose();
            }
            return list;
        }

        internal static List<Assignment> GetAssignments(int? userId = null, int? testId = null)
        {
            List<Assignment> list = new List<Assignment>();
            SQLiteDataReader dataReader = null;
            SQLiteCommand cmd = null;
            try
            {
                cmd = _conn.CreateCommand();
                if (userId != null)
                {
                    cmd.CommandText = "SELECT * FROM Assignments WHERE UserId = @user";
                    cmd.Parameters.AddWithValue("user", userId);
                }
                else
                {
                    cmd.CommandText = "SELECT * FROM Assignments WHERE TestId = @test";
                    cmd.Parameters.AddWithValue("test", testId);
                }
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    list.Add(new Assignment(dataReader.GetInt32(0),
                                            dataReader.GetInt32(1)));
                }
                dataReader.Close();
            }
            catch (Exception)
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
                cmd?.Dispose();
            }
            return list;
        }

        internal static List<Test> GetUserAssignments(int userId)
        {
            List<Test> list = new List<Test>();
            SQLiteDataReader dataReader = null;
            SQLiteCommand cmd = null;
            try
            {
                cmd = _conn.CreateCommand();
                cmd.CommandText = "SELECT Tests.TestId, Tests.TestName FROM Tests INNER JOIN Assignments ON Tests.TestId = (SELECT TestId FROM Assignments WHERE UserId = @user)";
                cmd.Parameters.AddWithValue("user", userId);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    list.Add(new Test(dataReader.GetInt32(0),
                                      dataReader.GetString(1),
                                      null));
                }
                dataReader.Close();
            }
            catch (Exception)
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
                cmd?.Dispose();
            }
            return list;
        }

        internal static string GetTestFolderLocation(int testId)
        {
            string location = "";
            string testName = "";
            SQLiteDataReader dataReader = null;
            SQLiteCommand cmd = null;
            try
            {
                cmd = _conn.CreateCommand();

                cmd.CommandText = "SELECT TestName, FolderLocation FROM Subjects AS s INNER JOIN (SELECT TestId, TestName, SubjectCode FROM TESTS WHERE TestId = @test) AS t ON s.SubjectCode = t.SubjectCode;";
                cmd.Parameters.AddWithValue("test", testId);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    testName = dataReader.GetString(0);
                    location = dataReader.GetString(1);
                }
                dataReader.Close();
            }
            catch (Exception)
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
                cmd?.Dispose();
            }
            return Path.Combine(location, testName);
        }

        internal static string GetTestFolderLocation(string testName)
        {
            string location = "";
            SQLiteDataReader dataReader = null;
            SQLiteCommand cmd = null;
            try
            {
                cmd = _conn.CreateCommand();

                cmd.CommandText = "SELECT FolderLocation FROM Subjects AS s INNER JOIN (SELECT TestName, SubjectCode FROM TESTS WHERE TestName = @test) AS t ON s.SubjectCode = t.SubjectCode;";
                cmd.Parameters.AddWithValue("test", testName);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    location = dataReader.GetString(0);
                }
                dataReader.Close();
            }
            catch (Exception)
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
                cmd?.Dispose();
            }
            return Path.Combine(location, testName);
        }

        internal static User GetLoginData(string username, string password)
        {
            User user = null;
            SQLiteDataReader dataReader = null;
            SQLiteCommand cmd = null;
            try
            {
                cmd = _conn.CreateCommand();

                cmd.CommandText = "SELECT * FROM Users WHERE Username = @user AND Password = @pass";
                cmd.Parameters.AddWithValue("user", username);
                cmd.Parameters.AddWithValue("pass", password);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    user = new User(username: dataReader.GetString(1),
                                      password: "",
                                      code: dataReader.GetString(3),
                                      roleId: dataReader.GetInt32(4),
                                      folderLocation: dataReader.GetString(5),
                                      userId: dataReader.GetInt32(0));
                }
                dataReader.Close();
            }
            catch (Exception)
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
                cmd?.Dispose();
            }
            return user;
        }

        internal static string GetRoleName(int roleId)
        {
            string roleName = null;
            SQLiteDataReader dataReader = null;
            SQLiteCommand cmd = null;
            try
            {
                cmd = _conn.CreateCommand();

                cmd.CommandText = "SELECT RoleName FROM Roles WHERE RoleId = @id";
                cmd.Parameters.AddWithValue("id", roleId);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    roleName = dataReader.GetString(0);
                }
                dataReader.Close();
            }
            catch (Exception)
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
                cmd?.Dispose();
            }
            return roleName;
        }

        internal static List<Test> GetUserAvailableTests(string subjectCode, int userId)
        {
            List<Test> tests = new List<Test>();
            SQLiteDataReader dataReader = null;
            SQLiteCommand cmd = null;
            try
            {
                cmd = _conn.CreateCommand();

                cmd.CommandText = "SELECT * FROM Tests as t INNER JOIN Assignments as a ON t.TestId = a.TestId WHERE t.SubjectCode = @code AND a.UserId = @userId";
                cmd.Parameters.AddWithValue("code", subjectCode);
                cmd.Parameters.AddWithValue("userId", userId);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    tests.Add(new Test(dataReader.GetInt32(0),
                                        dataReader.GetString(1),
                                        dataReader.GetString(2)));
                }
                dataReader.Close();
            }
            catch (Exception)
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
                cmd?.Dispose();
            }
            return tests;
        }

        internal static List<User> GetUsersOnTest(int testId)
        {
            List<User> users = new List<User>();
            SQLiteDataReader dataReader = null;
            SQLiteCommand cmd = null;
            try
            {
                cmd = _conn.CreateCommand();
                cmd.CommandText = "Select * from Users Where UserId In (SELECT UserId FROM Assignments WHERE TestId = @test)";
                cmd.Parameters.AddWithValue("test", testId);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    users.Add(new User(username: dataReader.GetString(1),
                                       password: "",
                                       code: dataReader.GetString(3),
                                       roleId: dataReader.GetInt32(4),
                                       folderLocation: dataReader.GetString(5),
                                       userId: dataReader.GetInt32(0)));
                }
                dataReader.Close();
            }
            catch (Exception)
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
                cmd?.Dispose();
            }
            return users;
        }
        #endregion

    }
}
