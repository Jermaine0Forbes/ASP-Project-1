# Logs

### 3-1-26

#### what are areas?
Areas in ASP.NET Core are a feature used to partition large web applications into smaller, manageable, and independent functional modules (e.g., Admin, Billing, Search). Each area mimics the MVC folder structure, containing its own Controllers, Views, and Models, allowing for better organization and enabling multiple developers to work on separate sections simultaneously

#### what the does code generator actually do and how do you connect with it properly

So from what I'm gathering, the code generator will allow generate the controller and data context that is need to connect to the database. When the database is connected that is when you have the ability to do CRUD operations and such. 

In the code provided by microsoft, I ran the example code like this and I obviously didn't get any results

`dotnet aspnet-codegenerator controller -name MoviesController -m Movie -dc MvcMovie.Data.MvcMovieContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlite`

Then I tried to customize like this, and tried to run it but it didn't work out because it was telling me `A type with the name User does not exist`. This is them basically saying that the model name `User` doesn't exist, and that's true because I named my model `UserModel`

`dotnet aspnet-codegenerator controller -name UsersController -m User -dc MvcUser.Data.MvcUserContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlserver`

This is the proper syntax you should make your code generator run successfully

`dotnet aspnet-codegenerator controller -name insertNameOfController -m insertNameOfModel -dc insertRootDirectoryName.Data.insertNameOfContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider insertNameOfDatabase`

So the final codegenerator command looked like this. This finally made it run

`dotnet aspnet-codegenerator controller -name UserController -m UserModel -dc WebApplication1.Data.MvcUserContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlserver`

#### so i messed up my web app

 Since I randomly am running different commands without knowing what I'm doing. I realized that I created two different DBContext's that I don't actually want to use. I have WebApplicationContext and UserContext,  the names are actually longer but the point is that I think I fucked it up. In any case, I'm going to proceed to remove them from Program.cs and then create a new db context called ApplicationDBContext


#### More than one DbContext was found. Specify which one to use. Use the '-Context' parameter for PowerShell commands and the '--context' parameter for dotnet commands.

In order to specify the specific context put this in the command
```
dotnet ef migrations add <MigrationName> --context <YourDbContextName>

```

You can also check the list of db contexts by running this command

```
dotnet ef dbcontext list

```

And if you want to remove a specific db context, then run this

```
dotnet ef migrations remove --context [YourDbContextName]

```

#### What does 'ef migrations add' do?

It creates a migration file for any of the models you included in the db context class, it also creates a designer file (don't know what that does), and a snapshot of the db context model( don't know why that is)

#### What is the Designer.cs when creating a migration

[From google](https://www.google.com/search?q=asp.net+core+what+is+the+designer+file+do+when+generating+a+migration&sca_esv=1ef01aa32e62b85d&sxsrf=ANbL-n4TRkpcDssngI1lQ6DYi3-Jk2mflQ%3A1772391103634&ei=v4qkaZq-Jrbgp84P-ajjyA8&biw=1523&bih=921&ved=0ahUKEwjamYqxr_-SAxU28MkDHXnUGPkQ4dUDCBE&uact=5&oq=asp.net+core+what+is+the+designer+file+do+when+generating+a+migration&gs_lp=Egxnd3Mtd2l6LXNlcnAiRWFzcC5uZXQgY29yZSB3aGF0IGlzIHRoZSBkZXNpZ25lciBmaWxlIGRvIHdoZW4gZ2VuZXJhdGluZyBhIG1pZ3JhdGlvbkiDhAFQ6xZY4ndwCngBkAEAmAHIAaABpDqqAQcxOC40Ni4xuAEDyAEA-AEBmAJCoALMNcICChAAGLADGNYEGEfCAgsQABiABBiRAhiKBcICBRAAGIAEwgIGEAAYFhgewgILEAAYgAQYhgMYigXCAgUQABjvBcICCBAAGKIEGIkFwgIIEAAYgAQYogTCAggQABgWGAoYHsICBRAhGKABwgIFECEYqwKYAwCIBgGQBgiSBwcxNi40OS4xoAfH2QKyBwY2LjQ5LjG4B7E1wgcJMTAuNDguNy4xyAeFAYAIAA&sclient=gws-wiz-serp):

In ASP.NET Core Entity Framework (EF) migrations, the auto-generated .Designer.cs file serves as a metadata file and an immutable snapshot of the model at the time the migration was created. It acts as a historical record of the expected database schema following that specific migration's application.

#### What is the ContextModelSnaphot when creating a migration

[From google:](https://www.google.com/search?q=asp.net+core+what+does+the+db+context+snapshot+do++when+generating+a+migration&sca_esv=1ef01aa32e62b85d&biw=1523&bih=921&sxsrf=ANbL-n7_ow-gSWsqMMAxtlFZSrqnyjwDmA%3A1772391620078&ei=xIykaba1BITjwN4P-cbsyAw&ved=0ahUKEwi2p6unsf-SAxWEMdAFHXkjG8kQ4dUDCBE&uact=5&oq=asp.net+core+what+does+the+db+context+snapshot+do++when+generating+a+migration&gs_lp=Egxnd3Mtd2l6LXNlcnAiTmFzcC5uZXQgY29yZSB3aGF0IGRvZXMgdGhlIGRiIGNvbnRleHQgc25hcHNob3QgZG8gIHdoZW4gZ2VuZXJhdGluZyBhIG1pZ3JhdGlvbkin0gFQvAhY3sQBcAp4AZABAJgBrwGgAecfqgEFMjQuMTe4AQPIAQD4AQGYAhSgAtIMwgIKEAAYsAMY1gQYR8ICBRAAGO8FwgIIEAAYogQYiQXCAggQABiABBiiBMICBBAhGAqYAwCIBgGQBgiSBwQxMS45oAfUjQGyBwM2Ljm4B8MMwgcGMi4xNi4yyAcjgAgA&sclient=gws-wiz-serp)

The DbContext snapshot file in ASP.NET Core Entity Framework (EF) Core migrations serves as a representation of the entire database schema as it existed after the last migration was applied

#### updating the database to current migration

`dotnet ef database update --context WebApplication1.Data.AppDBContext`

#### in order to create fake data

- In the models page create SeedData.cs
- download [Bogus](https://github.com/bchavez/Bogus?tab=readme-ov-file)
- Use bogus to generate fake data, here are [all the methods](https://github.com/bchavez/Bogus?tab=readme-ov-file#bogus-api-support) 

#### Error: Cannot insert explicit value for identity column in table 'UserModel' when IDENTITY_INSERT is set to OFF

[From google:](https://www.google.com/search?q=Cannot+insert+explicit+value+for+identity+column+in+table+%27UserModel%27+when+IDENTITY_INSERT+is+set+to+OFF&oq=Cannot+insert+explicit+value+for+identity+column+in+table+%27UserModel%27+when+IDENTITY_INSERT+is+set+to+OFF&gs_lcrp=EgZjaHJvbWUyBggAEEUYOdIBCDEyNDFqMGo3qAIAsAIA&sourceid=chrome&ie=UTF-8)
The error "Cannot insert explicit value for identity column in table 'UserModel' when IDENTITY_INSERT is set to OFF" occurs because you are attempting to provide a value for a column that SQL Server is configured to manage automatically. The column (likely the primary key, e.g., UserId) is an identity column, meaning the database is responsible for generating its value upon insertion

 #### helpful commands

 `dotnet aspnet-codegenerator controller -h`

 `dotnet ef migrations add InitialCreate`

 `dotnet add package Bogus`

 #### helpful links




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

dotnet aspnet-codegenerator controller -name UserController -m UserModel -dc WebApplication1.Data.MvcUserContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlserver


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

