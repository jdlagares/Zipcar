using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class UserManagerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagerController(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _userManager.GetUsersInRoleAsync("Customer"));
        }
    }
}