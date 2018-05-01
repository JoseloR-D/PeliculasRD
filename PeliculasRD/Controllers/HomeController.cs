using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PeliculasRD.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index() => View();
    }
}
