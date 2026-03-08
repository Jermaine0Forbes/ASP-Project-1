# Progress

Trying to keep track of the little things that I need to do 

- ~~figure out how to run the web app in the terminal~~
- ~~create a register and login page~~
- ~~display the register page as link~~
- ~~create a login controller~~ 
- ~~check if there's a form builder for asp.net~~
- ~~create a form in the register page~~
- ~~pass values of form into login controller~~
- ~~create a user model~~
- ~~connect to a database~~ 
- ~~seeding data into sqlserver~~
- ~~create view models for register, login, change password, and verify email~~
- ~~create an account controller that will handle the view models~~
- ~~possibly create a migration that will assign the Users to be an IdentityUser~~
- ~~update the login and register views to encorporate the properties in the RegisterViewModel and LoginViewModel~~
- ~~register a user~~
- ~~login a user~~
- set up .net debug mode
- add rate limiting to login
- add authorization annotation to routes
- create a post as a user
- list all posts for a user
- send an email
- create a policy
- create a claim
- deploy app to either do, aws, or azure
- set up NuGet packages on vscode


-[how to change a route path in a controller](https://www.google.com/search?q=asp.net+mvc+how+to+make+link+path+not+include+controller+name&sca_esv=c04783e6906b025b&sxsrf=ANbL-n5sCS8P_R9XLduMmMhJlTJWIzPySA%3A1770077964503&ei=DD-BaYmwHqeQp84Pkrbv4QU&biw=1523&bih=932&aic=0&ved=0ahUKEwiJ5rGihrySAxUnyMkDHRLbO1wQ4dUDCBE&uact=5&oq=asp.net+mvc+how+to+make+link+path+not+include+controller+name&gs_lp=Egxnd3Mtd2l6LXNlcnAiPWFzcC5uZXQgbXZjIGhvdyB0byBtYWtlIGxpbmsgcGF0aCBub3QgaW5jbHVkZSBjb250cm9sbGVyIG5hbWVI-JoEULjTAVjwmQRwCHgBkAEBmAHmAaABmDCqAQY5LjQ0LjG4AQPIAQD4AQGYAjygAvwuwgIKEAAYsAMY1gQYR8ICBhAAGBYYHsICCBAAGKIEGIkFwgIFEAAY7wXCAgUQIRigAcICBRAhGKsCwgIHECEYoAEYCpgDAIgGAZAGCJIHBTEzLjQ3oAeoygKyBwQ1LjQ3uAfmLsIHBjIuNTIuNsgHa4AIAA&sclient=gws-wiz-serp)

## commands

```cs

dotnet run

dotnet watch run

# to create an mvc app with identity services
dotnet new mvc -au Individual -o WebApplication1

# to create an app using razor pages with identity services
dotnet new webapp -au Individual -o WebApp1

# to create a blazor app with identity services
dotnet new blazor -au Individual -o BlazorApp1

# creates an empty controller
dotnet aspnet-codegenerator controller -name [CustomName]Controller -actions -api -outDir Controllers

# creates a new mvc app
dotnet  new mvc -o insertNameOfFolder

# creates a controller with CRUD operations that's related to the model
# and creates a database connection to an sql server
dotnet aspnet-codegenerator controller -name insertNameOfController -m insertNameOfModel -dc insertRootDirectoryName.Data.insertNameOfContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider insertNameOfDatabase

# installs faker for .net
dotnet add package Bogus

# creates a migration from the db context
dotnet ef migrations add InitialCreate

# shows the different db contexts 
dotnet ef dbcontext list

# create a migration file with a specific context 
dotnet ef migrations add <MigrationName> --context <YourDbContextName>

# remove a specific db context
dotnet ef migrations remove --context [YourDbContextName] 

# updating the database to the current migration
dotnet ef database update --context [YourDbContextName]

# just shows all the options/flags you can add when creating a controller
dotnet aspnet-codegenerator controller -h

# the initial tools you need to add if you are trying to use
# the entity framework or the codegenerator command cli
dotnet tool uninstall --global dotnet-aspnet-codegenerator
dotnet tool install --global dotnet-aspnet-codegenerator
dotnet tool uninstall --global dotnet-ef
dotnet tool install --global dotnet-ef

# packages you should add to your web app if you want to connect
# to a database, add migrations, and have a user identity
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.UI
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.SQLite
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```