using ICS_Test_Accounting.Tools;
using ICS_Test_Accounting.Services;
using System.Data.SqlClient;


var builder = WebApplication.CreateBuilder(args);

//DbUtility.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");



DbUtility.ConnectionString = $"Server={dbHost};Database={dbName};User ID=sa;Password={dbPassword}"; 
DbUtility.ConnectionStringNoDb = $"Server={dbHost};User ID=sa;Password={dbPassword}";
DbUtility.DbName = new SqlConnectionStringBuilder(DbUtility.ConnectionString)["Database"].ToString();



// Create&fill DB if doesn't exist
DbUtility.CreateDatabaseIfNull();
DbUtility.CreateTableEmployeeIfNull();
DbUtility.FillEmployeesIfEmpty();



// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IEmployeeService, EmployeeService>();



var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseHttpMethodOverride();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Employees}/{action=EmployeeList}/{id?}");

    app.MapControllerRoute(
    name: "home",
    pattern: "Employees");

});


app.Run();
