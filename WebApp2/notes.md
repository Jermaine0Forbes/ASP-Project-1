- if using vscode generate a new web app in the command like so
`dotnet new mvc -au Individual -o WebApp2`

- create a database connection to the sql server

```
dotnet add package Microsoft.AspNetCore.Identity.UI
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.SQLite
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools

```
- create a Post model class (Id, Title, Views, Likes, UserId, CreatedAt)
- attach the Post class to the AppDBContext class
- create a migration file
- create seeding class that will generate a number users and a post attached to them
- run the application to create the fake data
-