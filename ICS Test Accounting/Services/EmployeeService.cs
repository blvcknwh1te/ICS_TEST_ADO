using ICS_Test_Accounting.Models;
using System.Data.SqlClient;
using ICS_Test_Accounting.Tools;

namespace ICS_Test_Accounting.Services
{
    public interface IEmployeeService
    {
        List<Employee> GetEmployees();
        void PushEmployees(Employee employee);
        void DeleteEmployee(int id);
        void EditEmployee(Employee employee);
        Employee GetEmployeeById(int id);
        List<Employee> GetEmployeesByPost(string post);
        List<Employee> SortEmployeesByPost(List<Employee> employeeList);
        //List<Employee> SortEmployeesByColumn(List<Employee> employeeList,string column);
    }

    public class EmployeeService : IEmployeeService
    {
        public string connectionString { get; set; }
        public IConfiguration _configuration;
        public SqlConnection connection;
        string dbDateFormat = "MM.dd.yyyy";

        public EmployeeService(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");

        }

        public SqlCommand SelectEmployees(SqlConnection connection)
        {
            return new SqlCommand($"SELECT * FROM {DbUtility.DbName}.dbo.{DbUtility.EmployeesTableName}", connection);
        }

        public Employee GetEmployeeById(int id)
        {

            Employee employee = new Employee();

            var sqlQuery =
                (
                $"SELECT * FROM {DbUtility.DbName}.dbo.{DbUtility.EmployeesTableName}\n" +
                $"WHERE ID = {id}"
                );

            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var cmd = new SqlCommand(sqlQuery, connection);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    employee.ID = Convert.ToInt32(reader["Id"]);
                    employee.Name = reader["Name"].ToString();
                    employee.Surname = reader["Surname"].ToString();
                    employee.Post = reader["Post"].ToString();
                    employee.Salary = Convert.ToInt32(reader["Salary"]);
                    employee.BirthDate = Convert.ToDateTime(reader["BirthDate"]);
                }
            }
            return employee;
        }

        public List<Employee> GetEmployeesByPost(string post)
        {
            List<Employee> employeeList = new List<Employee>();

            var sqlQuery =
                (
                $"SELECT * FROM {DbUtility.DbName}.dbo.{DbUtility.EmployeesTableName}\n" +
                $"WHERE post = N'{post}'"
                );

            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var cmd=new SqlCommand(sqlQuery, connection);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var employee = new Employee();

                    employee.ID = Convert.ToInt32(reader["Id"]);
                    employee.Name = reader["Name"].ToString();
                    employee.Surname = reader["Surname"].ToString();
                    employee.Post = reader["Post"].ToString();
                    employee.Salary = Convert.ToInt32(reader["Salary"]);
                    employee.BirthDate = Convert.ToDateTime(reader["BirthDate"]);

                    employeeList.Add(employee);
                }
            }
            return employeeList;
        }

        public List<Employee> GetEmployees()
        {
            List<Employee> employeeList = new List<Employee>();

            try
            {
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var cmd = SelectEmployees(connection);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee employee = new Employee();
                        employee.ID = Convert.ToInt32(reader["Id"]);
                        employee.Name = reader["Name"].ToString();
                        employee.Surname = reader["Surname"].ToString();
                        employee.Post = reader["Post"].ToString();
                        employee.Salary = Convert.ToInt32(reader["Salary"]);
                        employee.BirthDate = Convert.ToDateTime(reader["BirthDate"]);

                        employeeList.Add(employee);
                    }
                }
                return employeeList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void PushEmployees(Employee employee)
        {
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var sqlQuery = ($"INSERT INTO {DbUtility.DbName}.dbo.{DbUtility.EmployeesTableName} " +
                    $"(Name,Surname,Post,BirthDate,Salary)\n" +
                    $"VALUES (N'{employee.Name}',N'{employee.Surname}',N'{employee.Post}','{employee.BirthDate.ToString(dbDateFormat)}',{employee.Salary})");

                var cmd = new SqlCommand(sqlQuery, connection);
                cmd.ExecuteNonQuery();
            }

        }

        public void DeleteEmployee(int id)
        {
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var cmd = new SqlCommand($"DELETE {DbUtility.DbName}.dbo.{DbUtility.EmployeesTableName} WHERE ID={id}", connection);
                cmd.ExecuteNonQuery();
            }
        }

        public void EditEmployee(Employee employee)
        {
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sqlQuery =
                    (
                    $"UPDATE {DbUtility.DbName}.dbo.{DbUtility.EmployeesTableName}\n" +
                    $"SET Name=N'{employee.Name}',Surname=N'{employee.Surname}',Post=N'{employee.Post}'," +
                    $"BirthDate='{employee.BirthDate.ToString(dbDateFormat)}',Salary={employee.Salary}\n" +
                    $"WHERE id={employee.ID}"
                    );
                var cmd = new SqlCommand(sqlQuery, connection);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Employee> SortEmployeesByPost(List<Employee> employeeList)
        {
            List<Employee> sortedEmployees = employeeList.OrderBy(emp => emp.Post).ToList();
            return sortedEmployees;
        }

    }
}
