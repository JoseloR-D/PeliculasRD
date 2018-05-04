using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PeliculasRD.Services.Interfaces;

namespace PeliculasRD.Controllers
{
    public class HomeController : Controller
    {
        IMovieRepository repository;
        public HomeController(IMovieRepository movieRepository)
        {
            repository = movieRepository;
        }

        public ViewResult Index()
        {
            var movies = repository.Movies();
            movies.Reverse();
            return View(movies);
        }
    }
}
