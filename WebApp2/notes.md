- if using vscode generate a new web app in the command like so
`dotnet new mvc -au Individual -o WebApp2`

- create a database connection to the sql server

1. First off download these packages if you have not done so already
```
dotnet add package Microsoft.AspNetCore.Identity.UI
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.SQLite
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools

```
2. We're going to create database in sql server. So in the `appsettings.json` create a connection string that will have the database name like so

` "AppDBContext": "Server=(localdb)\\mssqllocaldb;Database=WebApp;Trusted_Connection=True;MultipleActiveResultSets=true"`

3. Run `dotnet build` to check if there are any errors. If there not, then run `dotnet ef database update` to at least create the database

**Discovery**
You can create a new database in your local computer by just changing the name in the connection string. I did not know and I'm probably going to forget about it later on.

In anycase, the next step is to create the post that will have the views and the likes attached to the post

- create a Post model class (Id, Title, Body, Views, Likes, UserId, CreatedAt)
4. Create a Users and Posts class that can be added to the AppDBContext file
- attach the Post class to the AppDBContext class
- create a migration file
5. Create a migration file that will create the user's and post table. You can create a migration file with the cli like so 

`dotnet ef migrations Add insertName`

**If you're having migration issues**

If you are having a migration issue where you're having errors messages like this `  C:\wamp\www\asp\WebApp2\Data\Trash\ApplicationDbContextModelSnapshot.cs(13,33): error CS0111: Type 'AppDBContextModelSnapshot' already defines a member called 'BuildModel' with the same parameter types` then you should first drop your database `dotnet ef database drop --force`. And the second thing you need to do is delete all the contents in your migration folder `rm -rf ./Migrations`

6. Update the database

`dotnet ef database update`


- create seeding class that will generate a number users and a post attached to them

7. First install bogus 
`dotnet add package Bogus`

8. Create a Seeder file in the `Data` directory

9. With the bogus package, create a number of users and make sure each user has a post that is assigned to them

10. Go to the program file and allow the seeder class to be initialized below the bottom of the code that builds the app. Also, make sure you have your identity roles enabled as well

11. 


- run the application to create the fake data
- use websockets to increase the views of the post whenever someone visits it
- use websockets to allow the changing of likes of a post