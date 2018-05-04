using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeliculasRD.Services.Interfaces;
using PeliculasRD.Models;
using Microsoft.EntityFrameworkCore;

namespace PeliculasRD.Services.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        ApplicationDbContext context;
        public MovieRepository(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        public List<Movie> Movies() => context.Movies.ToList();

        public async Task<Movie> Movie(int id)
        {
            Movie movie = await context.Movies.SingleOrDefaultAsync(m => m.Id == id);

            if (movie != null)
                return movie;
            else
                return new Movie { };
        }

        public async Task<Movie> Create(Movie model)
        {
            var AddMovie = context.Movies.Add(model);
            await context.SaveChangesAsync();
            model.Id = AddMovie.Entity.Id;
            return model;
        }

        public async Task<Movie> Edit(Movie model)
        {
            context.Entry(model).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return model;
        }

        public async Task<Movie> Delete(Movie movie)
        {
            context.Movies.Remove(movie);
            await context.SaveChangesAsync();
            return movie;
        }
    }
}
