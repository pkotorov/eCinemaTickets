using eCinemaTickets.Data.Base;
using eCinemaTickets.Data.ViewModels;
using eCinemaTickets.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCinemaTickets.Data.Services
{
    public class MoviesService : EntityBaseRepository<Movie>, IMoviesService
    {
        private readonly AppDbContext context;

        public MoviesService(AppDbContext context)
            : base(context)
        {
            this.context = context;
        }

        public async Task AddNewMovieAsync(MovieViewModel data)
        {
            var movie = new Movie()
            {
                Name = data.Name,
                Description = data.Description,
                Price = data.Price,
                ImageUrl = data.ImageUrl,
                StartDate = data.StartDate,
                CinemaId = data.CinemaId,
                EndDate = data.EndDate,
                MovieCategory = data.MovieCategory,
                ProducerId = data.ProducerId
            };

            await this.context.Movies.AddAsync(movie);
            await this.context.SaveChangesAsync();

            foreach (var actorId in data.ActorIds)
            {
                var actorMovie = new ActorMovie()
                {
                    MovieId = movie.Id,
                    ActorId = actorId
                };

                await this.context.ActorsMovies.AddAsync(actorMovie);
            }

            await this.context.SaveChangesAsync();
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            var movieDetails = await this.context.Movies
                .Include(c => c.Cinema)
                .Include(p => p.Producer)
                .Include(am => am.ActorsMovies)
                .ThenInclude(a => a.Actor)
                .FirstOrDefaultAsync(n => n.Id == id);

            return movieDetails;
        }

        public async Task<MovieDropdownsViewModel> GetMovieDropdownsValuesAsync()
        {
            var response = new MovieDropdownsViewModel();
            response.Actors = await this.context.Actors.OrderBy(x => x.FullName).ToListAsync();
            response.Cinemas = await this.context.Cinemas.OrderBy(x => x.Name).ToListAsync();
            response.Producers = await this.context.Producers.OrderBy(x => x.FullName).ToListAsync();

            return response;
        }

        public async Task UpdateMovieAsync(MovieViewModel data)
        {
            var dbMovie = await this.context.Movies.FirstOrDefaultAsync(n => n.Id == data.Id);

            if(dbMovie != null)
            {
                dbMovie.Name = data.Name;
                dbMovie.Description = data.Description;
                dbMovie.Price = data.Price;
                dbMovie.ImageUrl = data.ImageUrl;
                dbMovie.StartDate = data.StartDate;
                dbMovie.CinemaId = data.CinemaId;
                dbMovie.EndDate = data.EndDate;
                dbMovie.MovieCategory = data.MovieCategory;
                dbMovie.ProducerId = data.ProducerId;

                await this.context.SaveChangesAsync();
            }

            var existingActorsDb = this.context.ActorsMovies.Where(n => n.MovieId == data.Id).ToList();
            this.context.ActorsMovies.RemoveRange(existingActorsDb);

            await this.context.SaveChangesAsync();

            foreach (var actorId in data.ActorIds)
            {
                var actorMovie = new ActorMovie()
                {
                    MovieId = data.Id,
                    ActorId = actorId
                };

                await this.context.ActorsMovies.AddAsync(actorMovie);
            }

            await this.context.SaveChangesAsync();
        }
    }
}
