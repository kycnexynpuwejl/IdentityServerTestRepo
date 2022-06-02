namespace IS4TEST.IdentityServer;

public class Startup
{
    public IWebHostEnvironment Environment { get; }

    public Startup(IWebHostEnvironment environment)
    {
        Environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // uncomment, if you want to add an MVC-based UI
        //services.AddControllersWithViews();

        services.AddIdentityServer(options =>
            {
                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;
            })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            // not recommended for production - you need to store your key material somewhere secure
            .AddDeveloperSigningCredential();
    }

    public void Configure(IApplicationBuilder app)
    {
        if (Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // uncomment if you want to add MVC
        //app.UseStaticFiles();
        //app.UseRouting();
        
        app.UseIdentityServer();

        // uncomment, if you want to add MVC
        //app.UseAuthorization();
        //app.UseEndpoints(endpoints =>
        //{
        //    endpoints.MapDefaultControllerRoute();
        //});
    }
}
