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
        IPasswordHasher<AppUser> passwordHasher;

        public AdminController(UserManager<AppUser> userMgr, IPasswordHasher<AppUser> passHash)
        {
            userManager = userMgr;
            passwordHasher = passHash;
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

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);

            if(user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                } else
                {
                    AddErrorsFromResult(result);
                }
            } else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            //Despues de guardar los errores ponmelos en la vista
            return View("Index", userManager.Users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);

            if (user != null)
                return View(user);
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, string email, string password)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            
            if(user != null)
            {
                user.Email = email;
                user.PasswordHash = passwordHasher.HashPassword(user, password);

                IdentityResult result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    AddErrorsFromResult(result);
            } else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(user);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach(IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
            
    }
}
