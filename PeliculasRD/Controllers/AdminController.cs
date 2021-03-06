﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PeliculasRD.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace PeliculasRD.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        UserManager<AppUser> userManager;
        IPasswordHasher<AppUser> passwordHasher;

        public AdminController(UserManager<AppUser> userMgr, IPasswordHasher<AppUser> passHash)
        {
            userManager = userMgr;
            passwordHasher = passHash;
        }

        [Authorize(Roles = "Admins")]
        public ViewResult Index() => View(userManager.Users);

        [Authorize(Roles = "Admins")]
        public ViewResult Categories() => View();

        [AllowAnonymous]
        public IActionResult Create()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Admins"))
                return View();
            else if (User.Identity.IsAuthenticated && User.IsInRole("Users"))
                return RedirectToAction("AccessDenied", "Account");
            else if (User.Identity.IsAuthenticated)
                return RedirectToAction("AccessDenied", "Account");
            return View();
        }

        //Docuemntacion.... Responsive Design    
        [HttpPost]
        [AllowAnonymous]
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
        [Authorize(Roles = "Admins")]
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

        public async Task<IActionResult> Edit(string nameOut)
        {
            if(User.Identity.Name == nameOut || User.IsInRole("Admins"))
            {
                AppUser user = await userManager.FindByNameAsync(nameOut);
                if (user != null)
                    return View(user);
                else
                    return RedirectToAction("Index");
            } else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
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
