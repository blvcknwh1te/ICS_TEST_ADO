namespace ICS_Test_Accounting.Models
{
    public class AllModels
    {
        public List<Employee> employeeList { get; set; }
        
        public List<string> GetPosts()
        {
            List<string> posts = new List<string>();
            if (employeeList != null)
            {
                foreach (var employee in employeeList)
                {
                    if (!posts.Contains(employee.Post))
                    {
                        posts.Add(employee.Post);
                    }
                }
            }
            return posts;
            
        }
    }
}
