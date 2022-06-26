using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

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
                _ = _conn.CloseAsync();
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
            List<string> list = new();

            cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table'";
            dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                list.Add(dataReader.GetString(0));
            }
            dataReader.Close();
            if (!list.Contains("Roles"))
            {
                cmd.CommandText = "Create Table Roles (RoleId Int Primary key, RoleName Varchar(50) Not null)";
                _ = cmd.ExecuteNonQuery();
            }
            if (!list.Contains("Users"))
            {
                cmd.CommandText = "Create Table Users (UserId Int Primary key, " +
                                                        "Username Varchar(100) Not null, " +
                                                        "Password Varchar(255) Not null, " +
                                                        "Code Varchar(10), " +
                                                        "RoleId Int Not null, " +
                                                        "FolderLocation Varchar(255)," +
                                                        "FOREIGN KEY(RoleId) REFERENCES Roles(RoleId))";
                _ = cmd.ExecuteNonQuery();
            }
            if (!list.Contains("Tests"))
            {
                cmd.CommandText = "Create Table Tests (TestId Int Primary key, TestName Varchar(100), SubjectCode Varchar(50), FOREIGN KEY(SubjectCode) REFERENCES Subjects(SubjectCode))";
                _ = cmd.ExecuteNonQuery();
            }
            if (!list.Contains("Subjects"))
            {
                cmd.CommandText = "Create Table Subjects (SubjectCode Varchar(50) Primary key, SubjectName Varchar(150), FolderLocation Varchar(255))";
                _ = cmd.ExecuteNonQuery();
            }
            if (!list.Contains("Assignments"))
            {
                cmd.CommandText = "Create Table Assignments (TestId Int, UserId Int, FOREIGN KEY(TestId) REFERENCES Tests(TestId), FOREIGN KEY(UserId) REFERENCES Users(UserId))";
                _ = cmd.ExecuteNonQuery();
            }
        }

        #region Insert Methods

        internal static void InsertUser(string username, string pass, string code, int role)
        {
            try
            {
                SQLiteCommand cmd = _conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Users(Username, Password, Code, RoleId, FolderLocation) VALUES (@name, @pass, @code, @role, @folder)";
                cmd.Parameters.AddWithValue("name", username);
                cmd.Parameters.AddWithValue("pass", pass);
                cmd.Parameters.AddWithValue("code", code);
                cmd.Parameters.AddWithValue("role", role);
                cmd.Parameters.AddWithValue("folder", "Users/" + username);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }

        }

        internal static void InsertSubject(string code, string name)
        {
            try
            {
                SQLiteCommand cmd = _conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Subjects(SubjectCode, SubjectName, FolderLocation) VALUES (@code, @name, @folder)";
                cmd.Parameters.AddWithValue("code", code);
                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("folder", "Subjects/" + code);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }

        }

        internal static void InsertTest(string testName, string subjectCode)
        {
            try
            {
                SQLiteCommand cmd = _conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Tests(TestName, SubjectCode) VALUES (@name, @code)";
                cmd.Parameters.AddWithValue("name", testName);
                cmd.Parameters.AddWithValue("code", subjectCode);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }

        }

        internal static void InsertAssignment(int userId, int testId)
        {
            try
            {
                SQLiteCommand cmd = _conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Assignments(UserId, TestId) VALUES (@user, @test)";
                cmd.Parameters.AddWithValue("name", userId);
                cmd.Parameters.AddWithValue("test", testId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }

        }

        #endregion

        #region Update Methods

        internal static void UpdateUser(string username, string pass, string code, int role, int userId)
        {
            try
            {
                SQLiteCommand cmd = _conn.CreateCommand();
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

        }

        internal static void UpdateSubject(string code, string name)
        {
            try
            {
                SQLiteCommand cmd = _conn.CreateCommand();
                cmd.CommandText = "Update Subjects SET SubjectName = @name WHERE SubjectCode = @code";
                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("code", code);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }

        }

        internal static void UpdateTest(string testName, string subjectCode, int testId)
        {
            try
            {
                SQLiteCommand cmd = _conn.CreateCommand();
                cmd.CommandText = "Update Tests SET TestName = @name, SubjectCode = @code WHERE TestId = @id";
                cmd.Parameters.AddWithValue("name", testName);
                cmd.Parameters.AddWithValue("code", subjectCode);
                cmd.Parameters.AddWithValue("id", testId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }

        }

        #endregion

        #region Delete Methods

        internal static void DeleteUser(int userId)
        {
            try
            {
                SQLiteCommand cmd = _conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Users WHERE UserId = @id";
                cmd.Parameters.AddWithValue("id", userId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }

        }

        internal static void DeleteSubject(string code)
        {
            try
            {
                SQLiteCommand cmd = _conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Subjects WHERE SubjectCode = @code";
                cmd.Parameters.AddWithValue("code", code);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }

        }

        internal static void DeleteTest(int testId)
        {
            try
            {
                SQLiteCommand cmd = _conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Tests WHERE TestId = @id";
                cmd.Parameters.AddWithValue("id", testId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }

        }

        internal static void DeleteAssignments(int userID, int testId)
        {
            try
            {
                SQLiteCommand cmd = _conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Assignments WHERE UserId = @user AND TestId = @test";
                cmd.Parameters.AddWithValue("user", userID);
                cmd.Parameters.AddWithValue("test", testId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }

        }

        #endregion

        #region Read Methods

        internal static List<User> GetUsers()
        {
            List<User> list = new();
            SQLiteDataReader dataReader;
            try
            {
                SQLiteCommand cmd = _conn.CreateCommand();

                cmd.CommandText = "SELECT * FROM Users";
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    list.Add(new User(dataReader.GetInt32(0),
                                        dataReader.GetString(1),
                                        dataReader.GetString(2),
                                        dataReader.GetString(3),
                                        dataReader.GetInt32(4),
                                        dataReader.GetString(5)));
                }
                dataReader.Close();
            }
            catch (Exception)
            {

            }
            return list;
        }

        internal static List<Role> GetRoles()
        {
            List<Role> list = new();
            SQLiteDataReader dataReader;
            try
            {
                SQLiteCommand cmd = _conn.CreateCommand();

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

            }
            return list;
        }

        internal static List<Subject> GetSubjects()
        {
            List<Subject> list = new();
            SQLiteDataReader dataReader;
            try
            {
                SQLiteCommand cmd = _conn.CreateCommand();

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

            }
            return list;
        }

        internal static List<Test> GetTests(string subjectCode)
        {
            List<Test> list = new();
            SQLiteDataReader dataReader;
            try
            {
                SQLiteCommand cmd = _conn.CreateCommand();

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

            }
            return list;
        }

        internal static List<Assignment> GetAssignments(int? userId = null, int? testId = null)
        {
            List<Assignment> list = new();
            SQLiteDataReader dataReader;
            try
            {
                SQLiteCommand cmd = _conn.CreateCommand();

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

            }
            return list;
        }

        internal static List<Test> GetUserAssignments(int userId)
        {
            List<Test> list = new();
            SQLiteDataReader dataReader;
            try
            {
                SQLiteCommand cmd = _conn.CreateCommand();
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

            }
            return list;
        }

        #endregion

    }
}
