# To create mvc web app in vs code:
## Create the app:
```csharp 
 dotnet new mvc -n InventoryApp
```
## Create a Controller:

```csharp 
cd .\Controllers\
dotnet new mvccontroller -n AuthController
```
## Create view page:
### The Auth directory is created
```csharp
 mkdir -p Views/Auth    //directly from root
```
### Then the view page has to be created manually:
Basic Template:
```csharp
@{
    ViewData["Title"] = "Login Page";
    Layout = "_LayoutAuth";
}

<div>
    <h1>Welcome To Login Page</h1>
    <p></p>
</div>
```
## Create Layout page:
### manually create it. Below is the basic template:
```csharp
<!DOCTYPE html>
<html>

<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width initial-scale=1.0 " />
    <title>@ViewData["Title"] - InventoryApp</title>
     <link rel="stylesheet" href="~/InventoryApp.styles.css" asp-append-version="true" />
    @RenderSection("Styles", required: false)
</head>
<body>
    <div>
        <main>
            @RenderBody()
        </main>
    </div>
</body>
</html>
```

## Now create css files for both view page and its master Layout page. 
### To create css file for _LayoutAuth.cshtml page:
create a _LayoutAuth.cshtml.cs page in same **Views/Shared/** directory where the layout page lies. You can add your custom css there.
For CSS isolation to work automatically, your CSS file needs to follow the exact naming convention:

- _Layout.cshtml.css ✅ (works)
- _LayoutAuth.cshtml.css ✅ (should work)
- Also Remember for it to work:
```csharp 
<link rel="stylesheet" href="~/InventoryApp.styles.css" asp-append-version="true" />
```
This line must be present in the header of _LayoutAuth.cshtml file.
This is the key! ASP.NET Core uses CSS isolation (also called scoped CSS) which bundles all the isolated CSS files into a single file named {ProjectName}.styles.css.


## To create css file for Login.cshtml page:

### Create **login.css** file under  **wwwroot** directory under **CSS** folder and create the necessary custom css.

### Must remember to add the pathname under @style section for login.cshtml file
```csharp
@section Styles {
    <link rel="stylesheet" href="~/css/login.css" />
}
```


# To connect to data base
```
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@123" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
```
## Step 1 — Pull SQL Server Docker Image
While making sure Docker is running, then in the terminal:
```
docker pull mcr.microsoft.com/mssql/server:2022-latest
```
## Step 2 — Run SQL Server Container
Run it with a password and exposed port:
```
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Inventory!Passw0rd" \
   -p 1433:1433 --name sqlserver2022 \
   -d mcr.microsoft.com/mssql/server:2022-latest
```
in powershell
```
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Inventory!Passw0rd" -p 1433:1433 --name sqlserver2022 -d mcr.microsoft.com/mssql/server:2022-latest
```

## Step 3 — Check if SQL Server is Running
```
docker ps
```
You should see `sqlserver2022` in the list.

## Step 4 — Connect to SQL Server (Optional GUI)  [This step is optional for now]
### If you want a GUI instead of typing SQL commands:  Install Azure Data Studio (lightweight, cross-platform)

New connection:

Server → localhost

Authentication → SQL Login

Username → sa

Password → YourStrong!Passw0rd

## Step 5 — Add Connection String in .NET Core
Inside your .NET Core project’s `appsettings.json`:

```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=MyAppDb;User Id=sa;Password=Inventory!Passw0rd;TrustServerCertificate=True;"
  }
}
```

## Step 6 — Install EF Core SQL Server Provider
In your terminal (inside project folder):
```
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```
## Step 7 — Create EF Core DbContext
Example Data/ApplicationDbContext.cs:
```
using Microsoft.EntityFrameworkCore;

namespace MyApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
    }
}
```
**Ofcourse, User model entity has to be created beforehand**

## Step 8 — Register DbContext in Program.cs
```
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```
Add at the top:
```
using Microsoft.EntityFrameworkCore;
```
### Step 9 — Run Migrations
```
dotnet ef migrations add InitialCreate
dotnet ef database update
```
**Ofcourse, User model entity has to be created beforehand**
**You can't run migration without an entity model**

# To integrate database 
## Create model in model directory.
example: User.cs
```
namespace InventoryApp.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLogIn { get; set; }
    public bool IsActive { get; set; } = true;
}
```

## Add database conext inside controller
At the top inside controller class:
```
 private readonly ApplicationDbContext _context;
        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }
```
## There are two ways to integrate a login page controller and function.
### The basic way:
An example of Auth controller code for login:
```
 [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var hashedPassword = ComputeSha256Hash(password);
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == hashedPassword);

            if (user != null)
            {
                ViewBag.Message = "Authorized";
                HttpContext.Session.SetString("Email", user.Email);
                return RedirectToAction("Dashboard", "Inventory");
            }
            ViewBag.Message = "Unauthorized. Please signup first if you dont have an account yet";
            return View();

        }
```
Accordingly The Login Page:
```
@{
    ViewData["Title"] = "Login Page";
    Layout = "_LayoutAuth";
}
@section Styles {
    <link rel="stylesheet" href="~/css/login.css" />
}

    <div class="login-header">
        <p>Please log in to continue.</p>
    </div>
    <div class="login-form-container">
        <form method="post" action="@Url.Action("Login", "Auth")">
        <label for="email" class="form-label">Email:</label>
        <input class="form-control" type='email' placeholder="Enter your Email" name="email"/>
        <label for="password" class="form-label">Password:</label>
        <input class="form-control" type='password' placeholder="Enter your password" name="password"/>
        <button class="login-button">Login</button>
        </form>
        <p>@ViewBag.Message</p>
    </div>
    <div class="signup-link">
        <p>Don't have an account?</p>
        <a asp-action="Signup" asp-controller="Auth">
       Register Now
        </a>
    </div>
```

### The practical/standard way
### UserViewModel in `ViewModels/SignupViewModel`
```
using System.ComponentModel.DataAnnotations;

namespace InventoryApp.ViewModels
{
    public class SignupViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
```
Accordingly Controller
Change your Signup actions to use the view model:
```
[HttpGet]
public ActionResult Signup()
{
    return View();
}

[HttpPost]
public ActionResult Signup(SignupViewModel model)
{
    if (!ModelState.IsValid)
    {
        return View(model);
    }

    if (_context.Users.Any(u => u.Email == model.Email))
    {
        ModelState.AddModelError("Email", "Email already registered.");
        return View(model);
    }

    var user = new User
    {
        Name = model.Name,
        Email = model.Email,
        PasswordHash = ComputeSha256Hash(model.Password),
    };

    _context.Users.Add(user);
    _context.SaveChanges();

    return RedirectToAction("Login");
}
```
SignUp Page:
```
@model InventoryApp.ViewModels.SignupViewModel

@{
    ViewData["Title"] = "Signup Page";
    Layout = "_LayoutAuth";
}

@section Styles {
    <link rel="stylesheet" href="~/css/login.css" />
}

<div class="login-header">
    <p>Please sign up to join us.</p>
</div>
<div class="login-form-container">
    <form asp-action="Signup" asp-controller="Auth" method="post">
        <div>
            <label asp-for="Name" class="form-label"></label>
            <input asp-for="Name" class="form-control" />
            <span asp-validation-for="Name" class="text-danger"></span>
        </div>

        <div>
            <label asp-for="Email" class="form-label"></label>
            <input asp-for="Email" class="form-control" />
            <span asp-validation-for="Email" class="text-danger"></span>
        </div>

        <div>
            <label asp-for="Password" class="form-label"></label>
            <input asp-for="Password" class="form-control" />
            <span asp-validation-for="Password" class="text-danger"></span>
        </div>

        <div>
            <label asp-for="ConfirmPassword" class="form-label"></label>
            <input asp-for="ConfirmPassword" class="form-control" />
            <span asp-validation-for="ConfirmPassword" class="text-danger"></span>
        </div>

        <button class="login-button">Signup</button>
    </form>
</div>
<div class="signup-link">
    <p>Already have an account?</p>
    <a asp-action="Login" asp-controller="Auth">Login Now</a>
</div>

@section Scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}
```