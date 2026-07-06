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
**Note**: if you create a User model that inherits the IdentityUser, you need to make sure that in the Progam.cs its being used in the AddDefaultIdentity method like so

```cs
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDBContext>();
builder.Services.AddControllersWithViews();
```

Also in `~/Views/Shared/_LoginPartial.cshtml` you need update the class in the SiginManager and UserManager services like so

```cs
@using Microsoft.AspNetCore.Identity
@inject SignInManager<User> SignInManager
@inject UserManager<User> UserManager
```
11. run the command `dotnet run` so that the seeder will run

12. Create a controller for the posts with the codegenerator command in the cli
`dotnet aspnet-codegenerator controller -name PostController -m WebApp2.Models.Post -dc AppDBContext -outDir Controllers`
 this will not only create a controller, but it will also create the views that would be connected to controller endpoints. To explain a little bit of flags names there is `-name` which is the name of the controller, every controller need to have Controller at the end of the name.  The `-m` is the name of the model, it's recommended to have the full namespace to find the model  like `WebApp2.Models.Post`. The `-dc` is the name of the DBContext class. The `-outDir` is defining what directory should the controller file be placed in 

- run the application to create the fake data
13.  Run `dotnet run` in order for the Seeder to run and then check your database to see if the data has been generated


14. Let's go to the default layout at `~/Views/_Layout.cshtml` and add a new link that will allow you to view all the posts
```html
<ul class="navbar-nav flex-grow-1">
    <li class="nav-item">
        <a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="Index">Home</a>
    </li>
    <li class="nav-item">
        <a class="nav-link text-dark" asp-area="" asp-controller="Post" asp-action="Index">Posts</a>
    </li>
    <li class="nav-item">
        <a class="nav-link text-dark" asp-area="" asp-controller="Home"
            asp-action="Privacy">Privacy</a>
    </li>
</ul>
```

15. Go to `~/Views/Post/Index` we should comment out the `Layout` keyword so that it will automatically inherit the established one. And then modify the table so that only the title, views, likes, and a link to view the page is shown.

16. Go to `~/Views/Post/Edit`, and comment out the `Layout` as well.  Then leave only title, body, views, and likes

17. Now with the Index and Edit pages, we have the basic properties that needed to be shown for this tutorial. Feel free to stylize the pages if you want them to look more presentable. An additional thing we are going to include is a code snippet from bootstrap(Since .net mvc uses the bootstrap library), that will be the buttons that will either up or down vote a post. Here is the code that you should put in between the **likes** value

```html
    <div class="btn-group" role="group" aria-label="Basic outlined example">
        <button type="button" class="btn btn-outline-primary plus">+</button>
        <button type="button" class="btn btn-outline-primary minus">-</button>
    </div>
```

18. Ok now here is the part where we start implementing websockets to send and receive data seamlessly. First we need to enable Websockets in our `Program` file, and do it before routing is being implemented like so 

```cs
// Enable WebSockets middleware BEFORE MVC routing
app.UseWebSockets(); 

app.UseRouting();
```

19. Next we want to create an endpoint that will allow receive and send information back to the websocket. So in the `PostController` we want to create a method called ViewSocket. This method will update the view count of a post and send the updated view count back to the post. We will need to add the HttpGet attribute and add the `/ws  ` so that when we send a message to the endpoint we can receive it.

- use websockets to increase the views of the post whenever someone visits it
- use websockets to allow the changing of likes of a post