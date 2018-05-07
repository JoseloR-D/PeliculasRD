using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using PeliculasRD.Models;


namespace PeliculasRD.Models
{
    public class SeedMovieData
    {

        public async static Task SeedMovies(IApplicationBuilder app, IConfiguration configuration)
        {
            //I do it like this and not like creatin a constructor method because I will habe to set
            //the ApplicationDbContext static, because this method to seed the data has to be static
            ApplicationDbContext context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();


            if (!context.Movies.Any())
            {
                context.Movies.AddRange(
                    new Movie {
                        Name = configuration["Data:MovieSeed:AmericanBeauty:Name"],
                        MovieUrl = configuration["Data:MovieSeed:AmericanBeauty:MovieUrl"],
                        Path = configuration["Data:MovieSeed:AmericanBeauty:Path"]
                    },
                    new Movie
                    {
                        Name = configuration["Data:MovieSeed:Avengers:Name"],
                        MovieUrl = configuration["Data:MovieSeed:Avengers:MovieUrl"],
                        Path = configuration["Data:MovieSeed:Avengers:Path"]
                    },
                    new Movie
                    {
                        Name = configuration["Data:MovieSeed:Rambo 3:Name"],
                        MovieUrl = configuration["Data:MovieSeed:Rambo 3:MovieUrl"],
                        Path = configuration["Data:MovieSeed:Rambo 3:Path"]
                    },
                    new Movie
                    {
                        Name = configuration["Data:MovieSeed:MeBeforeYou:Name"],
                        MovieUrl = configuration["Data:MovieSeed:MeBeforeYou:MovieUrl"],
                        Path = configuration["Data:MovieSeed:MeBeforeYou:Path"]
                    },
                    new Movie
                    {
                        Name = configuration["Data:MovieSeed:FagthersAnd:Name"],
                        MovieUrl = configuration["Data:MovieSeed:FagthersAnd:MovieUrl"],
                        Path = configuration["Data:MovieSeed:FagthersAnd:Path"]
                    },
                    new Movie
                    {
                        Name = configuration["Data:MovieSeed:FastFurios 8:Name"],
                        MovieUrl = configuration["Data:MovieSeed:FastFurios 8:MovieUrl"],
                        Path = configuration["Data:MovieSeed:FastFurios 8:Path"]
                    },
                    new Movie
                    {
                        Name = configuration["Data:MovieSeed:Black Parther:Name"],
                        MovieUrl = configuration["Data:MovieSeed:Black Parther:MovieUrl"],
                        Path = configuration["Data:MovieSeed:Black Parther:Path"]
                    },
                    new Movie
                    {
                        Name = configuration["Data:MovieSeed:JusticeLeage:Name"],
                        MovieUrl = configuration["Data:MovieSeed:JusticeLeage:MovieUrl"],
                        Path = configuration["Data:MovieSeed:JusticeLeage:Path"]
                    },
                    new Movie
                    {
                        Name = configuration["Data:MovieSeed:HarryPoter:Name"],
                        MovieUrl = configuration["Data:MovieSeed:HarryPoter:MovieUrl"],
                        Path = configuration["Data:MovieSeed:HarryPoter:Path"]
                    },
                    new Movie
                    {
                        Name = configuration["Data:MovieSeed:LesMiserables:Name"],
                        MovieUrl = configuration["Data:MovieSeed:LesMiserables:MovieUrl"],
                        Path = configuration["Data:MovieSeed:LesMiserables:Path"]
                    }
                );
            }
            await context.SaveChangesAsync();
        }
    }
}
