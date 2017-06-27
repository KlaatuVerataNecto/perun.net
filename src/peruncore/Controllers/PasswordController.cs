using infrastructure.user.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using peruncore.Config;
using peruncore.Models.User;

namespace peruncore.Controllers
{
    public class PasswordController : Controller
    {
        private IUserPasswordService _userPasswordService;
        private readonly AuthSettings _authSettings;
        public PasswordController(IUserPasswordService userPasswordService, IOptions<AuthSettings> authSettings)
        {
            _authSettings = authSettings.Value;
            _userPasswordService = userPasswordService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Forgot()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult BeginForgot(ForgotModel model)
        {
            var userReset = _userPasswordService.generateResetToken(model.email, _authSettings.ResetTokenLength, _authSettings.ExpiryDays);
            return View();
        }
    }
}