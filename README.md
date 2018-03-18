# Hello EF Core Scaffolding v 2.1

1. Open SSMS to connect to `(localdb)\MSSQLLocalDB`.
    - Create a new database named NorthwindSlim.
    - Download the NorthwindSlim script from: <http://bit.ly/northwindslim>.
    - Extract and execute the script in SSMS to create and populate tables.

2. Create ASP.NET Core 2.1 Web API project (with .Web suffix) in Visual Studio 2017 version 15.7 or greater.

3. Add .NET Core 2.1 Class Library project (with .Data suffix)
    - Add a reference to the Entities project
    - Add NuGet packages
    ```
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 2.1.0-preview1-final
    dotnet add package Microsoft.EntityFrameworkCore.Design --version 2.1.0-preview1-final
    ```
4. Add .NET Standard 2.0 Class Library (with .Entities suffix).

5. Edit the Data project csproj file, adding the following:
    ```xml
    <ItemGroup>
        <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.1.0-preview1-final" />
    </ItemGroup>
    ```
6. Open a command prompt at the Data project location and execute `dotnet restore`.

7. Run the `dotnet-ef` command to reverse engineer the NorthwindSlim database.
    ```
    dotnet ef dbcontext scaffold "Data Source=(localdb)\MSSQLLocalDB;initial catalog=NorthwindSlim;Integrated Security=True; MultipleActiveResultSets=True" Microsoft.EntityFrameworkCore.SqlServer  --output-dir "..\EfCore21Scaffold.Entities" --output-dbcontext-dir "."
    ```
    - Model classes will be generated in the Entities project, but you will need to change the namespace 
      from Data to Entities.

8. Remove the `OnConfiguring` method from `NorthwindSlimContext`.
    - Add the following constructors.
    ```csharp
    public NorthwindSlimContext() { }

    public NorthwindSlimContext(DbContextOptions options) : base(options) { }
    ```

9. Reference both the Entities and Data projects from the Web project.

10. Add a connection string to appSettings.json in the Web project.
    ```json
    "ConnectionStrings": {
    "NorthwindSlimContext": "Data Source=(localdb)\\MSSQLLocalDB;initial catalog=NorthwindSlim;Integrated Security=True; MultipleActiveResultSets=True"
    }
    ```

11. Add .NET Core 2.1 Class Library project (with .Data suffix)
    - Add a reference to the Entities project
    - Add NuGet package
    ```
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 2.1.0-preview1-final
    ```

12. Update the `Startup.ConfigureServices` method in the Web project.
    ```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc()
            .AddJsonOptions(options =>
                options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.All)
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        var connectionString = Configuration.GetConnectionString(nameof(NorthwindSlimContext));
        services.AddDbContext<NorthwindSlimContext>(options => options.UseSqlServer(connectionString));
    }
    ```

13. Add a customizable razor templates
    ```
    dotnet new -i "AspNetCore.WebApi.Templates::1.0.0-*"
    dotnet new webapi-templates
    ```

14. Add a controller, selecting API Controller with Actions using Entity Framework
    - Select `Product` class and `NorthwindSlimContext`
    - Press Ctrl+F5 to start the app and navigate to `api/products`.