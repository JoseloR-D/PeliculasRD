﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PeliculasRD.Models
{
    public class Movie
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string MovieUrl { get; set; }
        [Required]
        public string Path { get; set; }
    }
}
