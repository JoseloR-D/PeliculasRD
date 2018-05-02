using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PeliculasRD.Models;
using Microsoft.AspNetCore.Identity;

namespace PeliculasRD.Controllers
{
    public class AdminController : Controller
    {
        UserManager<AppUser> userManager;

        public AdminController(UserManager<AppUser> userMgr)
        {
            userManager = userMgr;
        }

        public ViewResult Index() => View(userManager.Users);

        public ViewResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUser model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.Name,
                    Email = model.Email
                };

                IdentityResult result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }
    }
}
