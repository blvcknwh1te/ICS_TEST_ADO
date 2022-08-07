using ICS_Test_Accounting.Models;
using ICS_Test_Accounting.Services;
using Microsoft.AspNetCore.Mvc;
using ICS_Test_Accounting.Tools;

namespace ICS_Test_Accounting.Controllers
{
    public class EmployeesController : Controller
    {

        private readonly IEmployeeService _employeeService;
        private AllModels _allModels;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
            _allModels = new AllModels();

        }

        [HttpGet]
        public IActionResult EmployeeList()
        {
            _allModels.employeeList = _employeeService.GetEmployees();
            _allModels.employeeList = _employeeService.SortEmployeesByPost(_allModels.employeeList);

            return View(_allModels);
        }
        [HttpGet("Employees/{post}")]
        public IActionResult EmployeeList(string post)
        {
            _allModels.employeeList = _employeeService.GetEmployeesByPost(post);
            _allModels.employeeList = _employeeService.SortEmployeesByPost(_allModels.employeeList);


            return View(_allModels);
        }


        [HttpPost("Employees/EditingPage/{id}")]
        public IActionResult EditingPage(int id)
        {
            var employee = _employeeService.GetEmployeeById(id);
            

            return View(employee);
        }

        [HttpGet("Employees/EditingPage/{id}")]
        [ActionName("EditingPage")]
        public IActionResult EditingPageGet(int id)
        {
            var employee = _employeeService.GetEmployeeById(id);


            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(int id,string name, string surname, string post, DateTime birthDate, int salary)
        {
            Employee employee = new Employee();
            employee.ID = id;
            employee.Name = name.Capitalize();
            employee.Surname = surname.Capitalize();
            employee.Post = post.Capitalize();
            employee.BirthDate = birthDate;
            employee.Salary = salary;

            _employeeService.EditEmployee(employee);

            return RedirectToAction("EmployeeList", "Employees");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _employeeService.DeleteEmployee(id);
            _allModels.employeeList = _employeeService.GetEmployees();

            return RedirectToAction("EmployeeList","Employees");
        }

        [HttpPost]
        public IActionResult AddingPage()
        {
            return View(_allModels);
        }

        [HttpGet("Employees/AddingPage")]
        [ActionName("AddingPage")]
        
        public IActionResult AddingPageGet()
        {
            return View(_allModels);
        }

        public IActionResult Add(string name,string surname,string post,DateTime birthDate,int salary)
        {
            Employee tmpEmployee = new Employee();
            tmpEmployee.Name = name.Capitalize();
            tmpEmployee.Surname = surname.Capitalize();
            tmpEmployee.Post = post.Capitalize();
            tmpEmployee.BirthDate = birthDate;
            tmpEmployee.Salary = salary;

            _employeeService.PushEmployees(tmpEmployee);

            return RedirectToAction("EmployeeList","Employees");
        }

        public IActionResult Filter(string post)
        {
            return RedirectToAction("EmployeeList", "Employees",new {post=post});
        }
    }
}
