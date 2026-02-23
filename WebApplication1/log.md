# Logs

### 2-22-26

how to run identity scaffolding 

```
dotnet aspnet-codegenerator identity -h
```

how to create identity scaffolding within terminal

```
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.UI
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools

```

how to create controllers, models, and datacontext through the terminal

```
dotnet aspnet-codegenerator controller -name MoviesController -m Movie -dc MvcMovie.Data.MvcMovieContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlite


dotnet aspnet-codegenerator controller -name UsersController -m User -dc MvcUser.Data.MvcUserContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlserver


dotnet aspnet-codegenerator controller -name UsersController -m User -dc MvcUser.Data.MvcUserContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlserver


```


### 2-20-26

to download packages that allow 

```
dotnet tool uninstall --global dotnet-aspnet-codegenerator
dotnet tool install --global dotnet-aspnet-codegenerator
dotnet tool uninstall --global dotnet-ef
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SQLite
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools

```


### 2-11-26

how to pass simple [data to route values](https://www.google.com/search?q=asp.net+pass+data+to+redirecttoaction&sca_esv=04aa8d85dd72ea04&biw=1523&bih=1278&sxsrf=ANbL-n7JsUAivB0zTDWUIebyzQ-MWUoZXQ%3A1770868393999&ei=qU6NacHVPLuNwbkPktXK6A0&ved=0ahUKEwiBzsXshtOSAxW7RjABHZKqEt0Q4dUDCBM&uact=5&oq=asp.net+pass+data+to+redirecttoaction&gs_lp=Egxnd3Mtd2l6LXNlcnAiJWFzcC5uZXQgcGFzcyBkYXRhIHRvIHJlZGlyZWN0dG9hY3Rpb24yBRAhGKABMgUQIRigATIFECEYoAEyBRAhGKABSLxlUIQPWMpfcAN4AZABAJgBwwGgAc8bqgEFMTIuMjG4AQPIAQD4AQGYAiSgAuAcwgIKEAAYsAMY1gQYR8ICBBAjGCfCAg4QABiABBiRAhixAxiKBcICCxAAGIAEGJECGIoFwgIFEAAYgATCAgoQABiABBgUGIcCwgIGEAAYFhgewgILEAAYgAQYhgMYigXCAgUQIRirAsICBxAhGKABGAqYAwCIBgGQBgiSBwUxMi4yNKAHvuABsgcEOS4yNLgH1hzCBwgwLjMwLjMuM8gHaYAIAA&sclient=gws-wiz-serp)

how to [add models](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/adding-model?view=aspnetcore-10.0&tabs=visual-studio-code) to asp.net core

Here is a link for [annotations](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-10.0) for data properties

