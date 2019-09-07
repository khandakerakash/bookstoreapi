## `RESTful API` developemnt using  [ASP.NET Core 2](https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-2.2)

### Here, I have created a simple `Bookstore` demo application for `RESTful API` practice purpose.<Enter>

**`Application Features`**

- I used [Postman](https://chrome.google.com/webstore/detail/postman/fhbjgbiflinjbdggehcddcbncdddomop) addons of google chrome browser for checking API data.

### And my instructor name is _[Biswa Nath Ghosh (Tapos)](https://github.com/tapos007)_ :relaxed:

### Development Setup
#### 1. Create DbContext class in Contexts Directory:

<code>
	public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
</code>

#### 2. Register the DbContext in the container of the `Startup` class `ConfigureServices` method:

<code>
	var connectionString = Configuration["ConnectionStrings:BookstoreDBConnectionString"];
    services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(connectionString));
</code>

- Then write the ConnectionStrings in appsettings.Development.json (For the Development Environment) file:
<code>
	"ConnectionStrings": {
		"BookstoreDBConnectionString": "Server=DESKTOP-ME7EDVE;Database=BookstoreApiDb;Trusted_Connection=True;MultipleActiveResultSets=true"
	}
</code>
