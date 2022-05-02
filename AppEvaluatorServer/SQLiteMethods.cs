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

    }
}
