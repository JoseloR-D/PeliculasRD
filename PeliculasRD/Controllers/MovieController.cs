﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PeliculasRD.Models;
using PeliculasRD.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.IO;


namespace PeliculasRD.Controllers
{
    [Authorize(Roles = "Admins")]
    public class MovieController : Controller
    {
        IMovieRepository repository;
        public MovieController(IMovieRepository movieRepository)
        {
            repository = movieRepository;
        }

        [AllowAnonymous]
        public ViewResult Index()
        {
            return View(repository.Movies());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Movie(int id)
        {
            Movie movie = await repository.Movie(id);

            if (movie != null)
                return View(movie);
            else
                return NotFound();
        }

        public ViewResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Movie movie, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file.Length != 0)
                {

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img",file.FileName);

                    using(var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    movie.Path = "/Img/" + file.FileName;
                    Movie model = await repository.Create(movie);

                    if(model != null)
                    {
                        return RedirectToAction("Index");
                    } else
                    {
                        return NotFound();
                    }
                } else
                {
                    ModelState.AddModelError("", "Please select an img");
                }
            }
            ModelState.AddModelError("", "Por favor llene los campos");
            return View(movie);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Movie model = await repository.Movie(id);
            if (model != null)
            {
                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Movie model)
        {
            if (ModelState.IsValid)
            {
                if(model != null)
                {
                    Movie movie = await repository.Edit(model);
                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound();
                }
            } else
            {
                ModelState.AddModelError("", "Por favor llene los campos");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Movie model = await repository.Movie(id);
            if(model != null)
            {
                Movie movie = await repository.Delete(model);
                if (movie != null)
                    return RedirectToAction("Index");
                else
                    return NotFound();
            }
            return NotFound();
        }
    }
}
