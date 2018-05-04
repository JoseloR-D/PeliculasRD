using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PeliculasRD.Models;
using PeliculasRD.Services.Interfaces;
using System.Threading.Tasks;

namespace PeliculasRD.Controllers
{
    public class MovieController : Controller
    {
        IMovieRepository repository;
        public MovieController(IMovieRepository movieRepository)
        {
            repository = movieRepository;
        }

        public ViewResult Movies()
        {
            return View(repository.Movies());
        }


    }
}
