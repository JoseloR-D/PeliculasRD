using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeliculasRD.Models;

namespace PeliculasRD.Services.Interfaces
{
    public interface IMovieRepository
    {
        IQueryable<Movie> Movies { get; }
        Task<Movie> Movie(int id);
        Task<Movie> Create(Movie movie);
        Task<Movie> Edit(Movie movie);
        Task<Movie> Delete(Movie movie);
    }
}
