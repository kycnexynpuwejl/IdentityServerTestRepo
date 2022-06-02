using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:7035";
        options.ClientId = "mvc";
        options.ClientSecret = "secret";
        options.ResponseType = "code";
        options.SaveTokens = true;
        options.Scope.Add("profile");
        options.GetClaimsFromUserInfoEndpoint = true;
        options.ClaimActions.MapUniqueJsonKey("myclaim1", "myclaim1");
    });


builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .RequireAuthorization();

app.Run();