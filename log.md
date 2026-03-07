# Logs


### 3-6-26

### the proper steps to migrate

- create migration `dotnet ef migrations add [insert name]`
- create seeder file in Models folder
- update database `dotnet ef database update`
- run dotnet `dotnet run`

### 3-5-26

#### data annotations that you can put on your model
[From google:](https://www.google.com/search?q=dotnet+model+datatype+annotations&sca_esv=286534660d0b6d5d&biw=1523&bih=921&sxsrf=ANbL-n61v38e-WL8w--ggYqYdLKI4HZnyg%3A1772749059584&ei=AwGqaciiI5Du-LYPurG66QU&ved=0ahUKEwiIsOPv5ImTAxUQN94AHbqYLl0Q4dUDCBE&uact=5&oq=dotnet+model+datatype+annotations&gs_lp=Egxnd3Mtd2l6LXNlcnAiIWRvdG5ldCBtb2RlbCBkYXRhdHlwZSBhbm5vdGF0aW9uczIHECEYoAEYCjIHECEYoAEYCjIHECEYoAEYCkjc_QRQ3ZkEWNT7BHACeACQAQGYAZ0BoAHKHaoBBDcuMja4AQPIAQD4AQGYAiKgAsUdqAIUwgIHECMYJxjqAsICEBAAGAMYtAIY6gIYjwHYAQHCAhYQLhjRAxgDGLQCGMcBGOoCGI8B2AEBwgILEAAYgAQYkQIYigXCAg4QLhiABBixAxjRAxjHAcICBRAAGIAEwgIOEAAYgAQYsQMYgwEYigXCAg4QLhiABBixAxiDARiKBcICBBAjGCfCAggQABiABBixA8ICCxAAGIAEGLEDGIMBwgIKEAAYgAQYQxiKBcICERAuGIAEGLEDGNEDGIMBGMcBwgIIEC4YgAQYsQPCAggQABiABBjJA8ICDRAAGIAEGLEDGBQYhwLCAgoQABiABBgUGIcCwgIREAAYgAQYkQIYsQMYgwEYigXCAggQABgWGAoYHsICBhAAGBYYHsICBRAhGKABwgIFECEYqwLCAgUQIRifBZgDDPEFFC7Sha1NLea6BgYIARABGAqSBwQ4LjI2oAfa1gGyBwQ2LjI2uAexHcIHCDMuMjYuNC4xyAdQgAgA&sclient=gws-wiz-serp)


**Validation Attributes**
- `[Required]`: Specifies that a property cannot be null or empty.
- `[StringLength(maximumLength, MinimumLength = minimumLength)]`: Defines the maximum and optionally minimum length of a string.
- `[MaxLength(maximumLength)] / [MinLength(minimumLength)]`: Similar to [StringLength] but only for max/min length.
- `[Range(minimum, maximum)]`: Validates that a numeric property falls within a specified range.
- `[RegularExpression(@"pattern")]`: Validates a property against a specific regular expression pattern.
- `[EmailAddress]`: Validates that the string has a valid email format.
- `[CreditCard]`: Validates that the string is a valid credit card number format.
- `[Phone]`: Validates that the string is a well-formed phone number.
- `[Url]`: Validates that the string is a valid URL.
- `[Compare("OtherProperty")]`: Validates that the property's value matches the value of another specified property (commonly used for password confirmation). 

[microsoft annotation glossary](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-10.0)

#### if you want to remove a dbcontext

The only way to do that is to remove the file that holds the dbcontext. Delete the file, and also remove any traces that are mentioned in other file.
Then you will truly be free from the old dbcontext

#### migration commands

[From google:](https://www.google.com/search?q=asp.net+core+migrate+down&oq=asp.net+core+migrate+down&gs_lcrp=EgZjaHJvbWUyBggAEEUYOTIHCAEQIRigATIHCAIQIRigATIHCAMQIRigAdIBCTIyMzU4ajBqN6gCALACAA&sourceid=chrome&ie=UTF-8)

to update the database to a new migration
`dotnet ef database update <Name-of-Target-Migration>`

to empty the database 
`dotnet ef database update 0`

to remove the last migration file 
`dotnet ef migrations remove`

### 3-3-26

#### what is the difference between razor pages and mvc

[From google:](https://www.google.com/search?q=what+is+the+difference+between+razor+pages+and+mvc&sca_esv=73000156de4ea350&biw=1523&bih=921&sxsrf=ANbL-n6zqXC34PusahYVuJDy0Y-p_XuYAg%3A1772561347814&ei=wyOnaZSCMYjcwN4PjoqVwAs&oq=what+is+the+difference+between+razor+pages+and+&gs_lp=Egxnd3Mtd2l6LXNlcnAiL3doYXQgaXMgdGhlIGRpZmZlcmVuY2UgYmV0d2VlbiByYXpvciBwYWdlcyBhbmQgKgIIADIFEAAYgAQyBhAAGBYYHjIGEAAYFhgeMgYQABgWGB4yCxAAGIAEGIYDGIoFMggQABiiBBiJBTIFEAAY7wUyCBAAGIAEGKIESIKGAVCuGliudHACeAGQAQCYAZcBoAHPJaoBBTI1LjIzuAEByAEA-AEBmAIyoALkJsICChAAGLADGNYEGEfCAgsQABiABBiRAhiKBcICDhAuGIAEGLEDGNEDGMcBwgILEAAYgAQYsQMYgwHCAgsQLhiABBixAxiDAcICCBAAGIAEGLEDwgIREC4YgAQYsQMY0QMYgwEYxwHCAgsQLhiABBjRAxjHAcICChAAGIAEGEMYigXCAhAQABiABBixAxhDGIMBGIoFwgIWEC4YgAQYsQMY0QMYQxiDARjHARiKBcICCxAAGIAEGJIDGIoFwgIOEAAYgAQYsQMYgwEYigXCAgoQABiABBgUGIcCwgIFEC4YgATCAhQQLhiABBiXBRjcBBjeBBjgBNgBAZgDAIgGAZAGCLoGBggBEAEYFJIHBTI3LjIzoAf26gKyBwUyNS4yM7gH3CbCBwYwLjQ2LjTIB3CACAA&sclient=gws-wiz-serp)

Razor Pages are page-focused and simplify development for simple, content-based UIs, while MVC provides a more flexible, scalable structure for large, complex enterprise applications.


#### how to create a .net app with identity authetication

to create an mvc app
`dotnet new mvc -au Individual -o WebApplication1`

to create an app using razor pages
`dotnet new webapp -au Individual -o WebApp1`

to create a blazor app
`dotnet new blazor -au Individual -o BlazorApp1`

#### how to create a controller with an entity model in cli

`dotnet aspnet-codegenerator controller -name [Entity]Controller -m [Entity] -dc [YourDbContext] --outDir Controllers`

#### how to create an empty controller in cli

`dotnet aspnet-codegenerator controller -name [CustomName]Controller -actions -api -outDir Controllers`
dotnet aspnet-codegenerator controller -name AccountController -m Users -dc AppDBContext --outDir Controllers

#### how to create an mvc app in the console

`dotnet  new mvc -o insertNameOfFolder`

#### how to create a .net project in visual studio code
- go into visual studio code and type (ctrl+shift+p)
- If .net options don't show up, then type .net
- Choose the option  `.Net: New Project...` > then choose whatever app
- Choose the location where you want the app to be
- Then name the app whatever

#### How change the display name of a model property

Add this annotation on top of the property like so
```cs
    [Display(Name="Created At")]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }
```

#### what is asp-validation-summary="ModelOnly"
[From google:](https://www.google.com/search?q=what+is+asp-validation-summary%3D%22ModelOnly%22&oq=what+is+asp-validation-summary%3D%22ModelOnly%22&gs_lcrp=EgZjaHJvbWUyBggAEEUYOTIHCAEQIRiPAjIHCAIQIRiPAjIHCAMQIRiPAtIBCDI3MzlqMGoxqAIAsAIA&sourceid=chrome&ie=UTF-8)

The asp-validation-summary="ModelOnly" attribute in ASP.NET Core is a Tag Helper used to display only model-level validation errors in a designated area (typically a <div> element), while excluding errors specific to individual properties.

#### Error : 'Users.Id' hides inherited member 'IdentityUser<string>.Id'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.

[From google:](https://www.google.com/search?q=%27Users.Id%27+hides+inherited+member+%27IdentityUser%3Cstring%3E.Id%27.+To+make+the+current+member+override+that+implementation%2C+add+the+override+keyword.+Otherwise+add+the+new+keyword.&oq=%27Users.Id%27+hides+inherited+member+%27IdentityUser%3Cstring%3E.Id%27.+To+make+the+current+member+override+that+implementation%2C+add+the+override+keyword.+Otherwise+add+the+new+keyword.&gs_lcrp=EgZjaHJvbWUyBggAEEUYOdIBBzczNWowajeoAgCwAgA&sourceid=chrome&ie=UTF-8)

The message "'Users.Id' hides inherited member 'IdentityUser.Id'" is a compiler warning in C# that occurs when a member in a derived class has the same name as a member in the base class, and the base member is not marked as virtual

My understanding: Basically I'm extending a class that are already has an Id as a property and in order to keep the Id. I need to change the data type of it and add the keyword override in front of it. Personally, I'm just going to remove all the properties that already exist


#### helpful links

- [mvc authentication repo](https://github.com/cloverluo112/WebApplication1)
- [Introduction to Identity on ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-10.0&tabs=net-cli)
- [Identity Migrations](https://www.tutorialspoint.com/asp.net_core/asp.net_core_identity_migrations.htm)

### 3-1-26

#### what are areas?
Areas in ASP.NET Core are a feature used to partition large web applications into smaller, manageable, and independent functional modules (e.g., Admin, Billing, Search). Each area mimics the MVC folder structure, containing its own Controllers, Views, and Models, allowing for better organization and enabling multiple developers to work on separate sections simultaneously

#### what the does code generator actually do and how do you connect with it properly

So from what I'm gathering, the code generator will allow generate the controller and data context that is need to connect to the database. When the database is connected that is when you have the ability to do CRUD operations and such. 

In the code provided by microsoft, I ran the example code like this and I obviously didn't get any results

`dotnet aspnet-codegenerator controller -name MoviesController -m Movie -dc MvcMovie.Data.MvcMovieContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlite`

Then I tried to customize like this, and tried to run it but it didn't work out because it was telling me `A type with the name User does not exist`. This is them basically saying that the model name `User` doesn't exist, and that's true because I named my model `Users`

`dotnet aspnet-codegenerator controller -name UsersController -m User -dc MvcUser.Data.MvcUserContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlserver`

This is the proper syntax you should make your code generator run successfully

`dotnet aspnet-codegenerator controller -name insertNameOfController -m insertNameOfModel -dc insertRootDirectoryName.Data.insertNameOfContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider insertNameOfDatabase`

So the final codegenerator command looked like this. This finally made it run

`dotnet aspnet-codegenerator controller -name UserController -m Users -dc WebApplication1.Data.MvcUserContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlserver`

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

#### Error: Cannot insert explicit value for identity column in table 'Users' when IDENTITY_INSERT is set to OFF

[From google:](https://www.google.com/search?q=Cannot+insert+explicit+value+for+identity+column+in+table+%27Users%27+when+IDENTITY_INSERT+is+set+to+OFF&oq=Cannot+insert+explicit+value+for+identity+column+in+table+%27Users%27+when+IDENTITY_INSERT+is+set+to+OFF&gs_lcrp=EgZjaHJvbWUyBggAEEUYOdIBCDEyNDFqMGo3qAIAsAIA&sourceid=chrome&ie=UTF-8)
The error "Cannot insert explicit value for identity column in table 'Users' when IDENTITY_INSERT is set to OFF" occurs because you are attempting to provide a value for a column that SQL Server is configured to manage automatically. The column (likely the primary key, e.g., UserId) is an identity column, meaning the database is responsible for generating its value upon insertion

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

dotnet aspnet-codegenerator controller -name UserController -m Users -dc WebApplication1.Data.MvcUserContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlserver


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

