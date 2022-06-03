using System.Reflection;
using IdentityServerHost.Quickstart.UI;
using Microsoft.EntityFrameworkCore;

namespace IS4TEST.IdentityServer;

public class Startup
{
    public IWebHostEnvironment Environment { get; }

    public Startup(IWebHostEnvironment environment)
    {
        Environment = environment;
    }
    
    string? migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
    const string connectionString = "Data Source=IS4.db";

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();

        services.AddIdentityServer()
            .AddTestUsers(TestUsers.Users)
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlite(connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlite(connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddDeveloperSigningCredential();

    }

    public void Configure(IApplicationBuilder app)
    {
        if (Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        //app.InitializeDatabase();
        app.UseStaticFiles();
        app.UseRouting();
        
        app.UseIdentityServer();

        
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        { 
            endpoints.MapDefaultControllerRoute();
        });
    }
}
