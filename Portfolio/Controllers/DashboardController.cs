using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Services;

namespace Portfolio.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IUserService userService, ILogger<DashboardController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var userEmail = User.Identity?.Name;
            var displayName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;

            ViewBag.UserEmail = userEmail;
            ViewBag.DisplayName = displayName;

            return View();
        }
    }
}