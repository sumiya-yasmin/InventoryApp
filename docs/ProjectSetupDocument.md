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