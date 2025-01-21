using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;

namespace StorageWeb.Controllers;

public class TestController : Controller
{
    // 
    // GET: /Test/
    public string Index()
    {
        return "This is my default action...";
    }
    // 
    // GET: /Test/Welcome/ 
    public string Welcome(string name, int id = 1)
    {
        return HtmlEncoder.Default.Encode($"Hello {name}, ID: {id}");
    }
}