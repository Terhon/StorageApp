using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;

namespace StorageWeb.Controllers;

public class TestController : Controller
{
    // 
    // GET: /Test/
    public IActionResult Index()
    {
        return View();
    }
    // 
    // GET: /Test/Welcome/ 
    public IActionResult Welcome(string name, int num = 1)
    {
        ViewData["Message"] = "Hello " + name;
        ViewData["Num"] = num;
        return View();
    }
}