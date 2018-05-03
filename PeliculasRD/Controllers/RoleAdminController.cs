using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PeliculasRD.Models;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace PeliculasRD.Controllers
{
    [Authorize(Roles = "Admins")]
    public class RoleAdminController : Controller
    {
        RoleManager<IdentityRole> roleManager;
        UserManager<AppUser> userManager;

        public RoleAdminController(RoleManager<IdentityRole> roleMgr, UserManager<AppUser> userMgr)
        {
            roleManager = roleMgr;
            userManager = userMgr;

        }

        public ViewResult Index() => View(roleManager.Roles);

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Required] string name)
        {
            if(ModelState.IsValid)
            {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                } else
                {
                    AddErrorsFromResult(result);
                }

            } 

            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);

            if(role != null)
            {
                IdentityResult result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                } else
                {
                    AddErrorsFromResult(result);
                }
            } else
            {
                ModelState.AddModelError("", "Role not Found");
            }
            return View("Index", roleManager.Roles);
        }

        //Most of the code in the GET version of the Edit action method is responsible for 
        //generating the sets of members and nonmembers of the selected role
        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            List<AppUser> members = new List<AppUser>();
            List<AppUser> noMembers = new List<AppUser>();

            foreach(AppUser user in userManager.Users)
            {
                var users = await userManager.IsInRoleAsync(user, role.Name) ? members : noMembers;
                users.Add(user);
            }

            return View(new RoleEditModel { Role = role, Members = members, NoMembers = noMembers });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleModificationModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                //Users to add
                foreach(string userId in model.IdsToAdd ?? new string[] { })
                {
                    AppUser user = await userManager.FindByIdAsync(userId);
                    if(user != null)
                    {
                        result = await userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            AddErrorsFromResult(result);
                    }
                }

                //Users to Delete
                foreach(string userId in model.IdsToDelete ?? new string[] { })
                {
                    AppUser user = await userManager.FindByIdAsync(userId);
                    if(user != null)
                    {
                        result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            AddErrorsFromResult(result);
                    }
                }
            }

            if (ModelState.IsValid)
                return RedirectToAction("Index");
            else
                return await Edit(model.RoleId);   // Go to the get method of the Edit Method
        }

        public void AddErrorsFromResult(IdentityResult result)
        {
            foreach(IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }  
}
