using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using persistance.dapper.repository;
using ui.web.Config;
using ui.web.Infrastructure.Auth;
using ui.web.Models.User;

namespace ui.web.Controllers
{
    public class LoginController : Controller
    {
        private IUserQueries _userQueries;

        public LoginController(IUserQueries userQueries)
        {
            _userQueries = userQueries;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Index(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var user1 =_userQueries.Get("user", "pass");

            // TODO: Get object from database
            dynamic user = new System.Dynamic.ExpandoObject();
            user.id = 1;
            user.username = "klaatuveratanecto";
            user.email = model.email;
            user.roles = "localuser";
            user.avatar = "placeholder.jpg";

            HttpContext.Authentication.SignInAsync(ConfigVariables.AuthSchemeName, ClaimsPrincipalFactory.Build(user.id, user.username, model.email, user.roles, user.avatar));
            return RedirectToAction("Index", "Home");
        }
    }
}