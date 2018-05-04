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

        public IQueryable<Movie> Movies => context.Movies;

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
            context.Movies.Add(model);
            await context.SaveChangesAsync();
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
