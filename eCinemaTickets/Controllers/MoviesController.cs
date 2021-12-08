using eCinemaTickets.Data;
using eCinemaTickets.Data.Services;
using eCinemaTickets.Data.Static;
using eCinemaTickets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCinemaTickets.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class MoviesController : Controller
    {
        private readonly IMoviesService moviesService;

        public MoviesController(IMoviesService moviesService)
        {
            this.moviesService = moviesService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var movies = await this.moviesService.GetAllAsync(n => n.Cinema);
            return this.View(movies);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Filter(string searchString)
        {
            var movies = await this.moviesService.GetAllAsync(n => n.Cinema);

            if(!string.IsNullOrEmpty(searchString))
            {
                var filteredResult = movies.Where(x => x.Name.ToLower().Contains(searchString.ToLower()) || x.Description.ToLower().Contains(searchString.ToLower())).ToList();

                return this.View(nameof(Index), filteredResult);
            }

            return this.View(nameof(Index), movies);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var movieDetail = await this.moviesService.GetMovieByIdAsync(id);

            if (movieDetail == null)
            {
                return this.View(nameof(NotFound));
            }

            return this.View(movieDetail);
        }

        public async Task<IActionResult> Create()
        {
            var movieDropdownsData = await this.moviesService.GetMovieDropdownsValuesAsync();

            ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MovieViewModel movie)
        {
            if (!ModelState.IsValid)
            {
                var movieDropdownsData = await this.moviesService.GetMovieDropdownsValuesAsync();
                ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");
                return this.View(movie);
            }

            await this.moviesService.AddNewMovieAsync(movie);

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var movieDetails = await this.moviesService.GetMovieByIdAsync(id);

            if (movieDetails == null)
            {
                return this.View(nameof(NotFound));
            }

            var response = new MovieViewModel()
            {
                Id = movieDetails.Id,
                Name = movieDetails.Name,
                Description = movieDetails.Description,
                Price = movieDetails.Price,
                ImageUrl = movieDetails.ImageUrl,
                MovieCategory = movieDetails.MovieCategory,
                CinemaId = movieDetails.CinemaId,
                ProducerId = movieDetails.ProducerId,
                ActorIds = movieDetails.ActorsMovies.Select(n => n.ActorId).ToList(),
                StartDate = movieDetails.StartDate,
                EndDate = movieDetails.EndDate,
            };

            var movieDropdownsData = await this.moviesService.GetMovieDropdownsValuesAsync();
            ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");

            return this.View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, MovieViewModel movie)
        {
            if (id != movie.Id)
            {
                return this.View(nameof(NotFound));
            }

            if (!ModelState.IsValid)
            {
                var movieDropdownsData = await this.moviesService.GetMovieDropdownsValuesAsync();
                ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");
                return this.View(movie);
            }

            await this.moviesService.UpdateMovieAsync(movie);

            return this.RedirectToAction(nameof(Index));
        }
    }
}
