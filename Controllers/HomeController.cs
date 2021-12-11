using AuthNET6.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace AuthNET6.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        [AllowAnonymous]
        public IActionResult Index(bool erroLogin)
        {

            if (erroLogin)
            {
                ViewBag.Erro = "Nickname e/ou senha estão incorretos";
            }
            if (HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Profile");
            return View();
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login(User user)
        {
            var usuarioDB = new User()
            {
                nickname = "eduardo",
                password = "123456",
                cargo = ""
            };

            if (!usuarioDB.nickname.Equals(user.nickname) ||
                !usuarioDB.password.Equals(user.password)
                )
            {
                return RedirectToAction("Index", new { erroLogin = true});
            }

            await new Services().Login(HttpContext, user);
            return RedirectToAction("Profile");
        }

        [Authorize]
        public async Task<IActionResult> Sair()
        {
            await new Services().Logoff(HttpContext);
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Profile()
        {
            ViewBag.Permissoes = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value);
            return View();
        }


    }
}