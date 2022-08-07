using ICS_Test_Accounting.Models;
using System.Data.SqlClient;

namespace ICS_Test_Accounting.Tools
{
    public class DbUtility
    {
        public static string ConnectionString { get; set; }
        public static string ConnectionStringNoDb { get; set; }
        
        public static string DbName { get; set; }
        public static string EmployeesTableName = "Employees";


        public static void CreateDatabaseIfNull()
        {

            SqlCommand cmd = null;
            using (var connection = new SqlConnection(ConnectionStringNoDb))
            {
                connection.Open();

                using (cmd = new SqlCommand($"If(db_id(N'{DbName}') IS NULL) CREATE DATABASE [{DbName}]", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        
        public static void FillEmployeesIfEmpty()
        {
            var sqlQuery = 
                (
                $"IF NOT EXISTS(SELECT 1 FROM {DbName}.dbo.{EmployeesTableName})\n" +
                $"BEGIN\n"+
                $"INSERT INTO {DbName}.dbo.{EmployeesTableName} VALUES\n" +
                $"(N'Sergey', N'Кузьменков', N'Менеджер', '2001-05-31', 300)," +
                $"(N'Юлия', N'Стрельцова', N'Менеджер', '2000-07-06', 400)," +
                $"(N'Владимир', N'Орехов', N'Программист', '1999-09-15', 500)\n" +
                $"END;"
                );

            using(var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var cmd = new SqlCommand(sqlQuery, connection);
                cmd.ExecuteNonQuery();
            }
        }

        public static void CreateTableEmployeeIfNull()
        { 
            var tableName = EmployeesTableName;


            var sqlQuery =
                (
                $"IF NOT EXISTS (SELECT * FROM {DbName}.INFORMATION_SCHEMA.TABLES \n" +
                $"WHERE table_name='{tableName}')\n" +
                $"BEGIN\n" +
                $"CREATE TABLE {tableName} " +
                $"(\n" +
                "ID int IDENTITY(1,1) NOT NULL," +
                "Name nvarchar(50) NOT NULL," +
                "Surname nvarchar(50) NULL," +
                "Post nvarchar(50) NULL," +
                "BirthDate datetime2(1) NOT NULL," +
                "Salary int NULL" +
                ")\n" +
                "END"
                );

            Console.WriteLine(sqlQuery);

            SqlCommand cmd =  null;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (cmd = new SqlCommand(sqlQuery,connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        

    }
}
