# EF Core + SQL Server

## Goal

Add .NET EF Core to .NET Core WebApi project

## Content

- [EF Core + SQL Server](#ef-core--sql-server)
  - [Goal](#goal)
  - [Content](#content)
  - [Prerequisites](#prerequisites)
  - [WebApi](#webapi)
  - [Install EF Core](#install-ef-core)
  - [DBContext](#dbcontext)
  - [Create Database from Context](#create-database-from-context)
  - [Database Access (CRUD)](#database-access-crud)
  - [Tips and Tricks](#tips-and-tricks)
    - [EF Command line Tools + Migrations](#ef-command-line-tools--migrations)
  - [Whats next](#whats-next)
  - [Additional Information](#additional-information)
    - [Links](#links)
    - [Current Versions](#current-versions)

## Prerequisites

- SQL Server
- Recommended: SQL Server Management Studio (SSMS)
- Your user must be able to login to the sql server and create databases and tables
- Alternatively you can change the connectionstring from "Trusted_Connection=True" to user/password (DBContext)

## WebApi

Create a .NET Core WebApi project. Or use this: <https://github.com/boeschenstein/angular9-dotnetcore3>

## Install EF Core

open cmd in the folder of the `csproj` file and enter:

``` cmd
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

## DBContext

Create a Folder EF and add 3 classes:

``` c#
namespace MyBackend.EF
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        // replace BloggingEFTest, if you want a different name for your database
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BloggingEFTest;Trusted_Connection=True;MultipleActiveResultSets=true;");
    }
}
```

``` c#
namespace MyBackend.EF
{
    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }

        // Referential Integrity: with this line, EF will generate a foreign key
        public List<Post> Posts { get; } = new List<Post>();
    }
}
```

``` c#
namespace MyBackend.EF
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
```

## Create Database from Context

``` cmd
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Start SQL Server Management Studio (SSMS), login to your database server and check the new database.

## Database Access (CRUD)

To check th implementation, run the following Code.

For simplicity reasons, I added the following code to my Get() in WeatherForecastController.

``` c#
using MyBackend.EF;
using System.Linq;

...

using (var db = new BloggingContext())
{
    // Create
    Console.WriteLine("Inserting a new blog");
    db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
    db.SaveChanges();

    // Read
    Console.WriteLine("Querying for a blog");
    var blog = db.Blogs
        .OrderBy(b => b.BlogId)
        .First();

    // Update
    Console.WriteLine("Updating the blog and adding a post");
    blog.Url = "https://devblogs.microsoft.com/dotnet";
    blog.Posts.Add(
        new Post
        {
            Title = "Hello World",
            Content = "I wrote an app using EF Core!"
        });
    db.SaveChanges();

    // Delete
    Console.WriteLine("Delete the blog");
    db.Remove(blog);
    db.SaveChanges();
}
```

>Please remove this code after your test: It is not recommended to access database directly in controller!

Congratulations, you just added EF Core to your ASP.NET WebAPI project!

## Tips and Tricks

### EF Command line Tools + Migrations

``` cmd
dotnet tool install --global dotnet-ef
```

Update tooling

``` cmd
dotnet tool update --global dotnet-ef
```

To add a new migration

``` cmd
dotnet ef migrations add {name}

rem Add project, if solution contains many
dotnet ef migrations add {name} --context MyDbContext --startup-project <MyProjectName>
```

To remove last migration file from project

``` cmd
dotnet ef migrations remove

rem Add project, if solution contains many
dotnet ef migrations remove --context MyDbContext --startup-project <MyProjectName>
```

Revert to old migration

``` cmd
dotnet ef database update {old_migration}

rem Add project, if solution contains many
dotnet ef database update {old_migration} --context MyDbContext --startup-project <MyProjectName>
```

To update DB to the latest migration

``` cmd
dotnet ef database update

rem Add project, if solution contains many
dotnet ef database update  --context MyDbContext --startup-project <MyProjectName>
```

## Whats next

Swagger/OpenApi are tools which can create your Angular code to access the backend: check this <https://github.com/boeschenstein/angular9-dotnetcore-openapi-swagger>

## Additional Information

### Links

- EF Core: <https://docs.microsoft.com/en-us/ef/>
- Command line Tools: <https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet>
- Migrations: <https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/migrations?view=aspnetcore-3.1>
- ASP.NET WebApi: <https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.1&tabs=visual-studio>
- Angular: <https://angular.io/>
- About me: <https://github.com/boeschenstein>

### Current Versions

- Visual Studio 2019 16.5.3
- .NET core 3.1
- npm 6.14.4
- node 12.16.1
- Angular CLI 9.1
