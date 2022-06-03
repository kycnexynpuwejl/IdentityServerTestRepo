using System.Diagnostics;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using IS4TEST.MVC.Models;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json.Linq;

namespace IS4TEST.MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    public IActionResult Logout()
    {
        return SignOut("Cookies", "oidc");
    }
    
    public async Task<IActionResult> CallApi()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var content = await client.GetStringAsync("https://localhost:7273/identity");
        ViewBag.Json = JArray.Parse(content).ToString();
        return View("json");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}