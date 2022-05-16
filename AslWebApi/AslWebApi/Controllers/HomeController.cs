using AslWebApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AslWebApi.Controllers
{
    [Route("[controller]/[action]/")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginModel loginModel)
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            LoginModel loginModel = new LoginModel()
            {
                UserName = "Rahim",
                Password = "123"
            };
            return View(loginModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            
            return View(loginModel);
        }

    }
}
