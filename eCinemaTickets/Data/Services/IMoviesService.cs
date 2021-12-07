using eCinemaTickets.Data.Base;
using eCinemaTickets.Data.ViewModels;
using eCinemaTickets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCinemaTickets.Data.Services
{
    public interface IMoviesService : IEntityBaseRepository<Movie>
    {
        Task<Movie> GetMovieByIdAsync(int id);

        Task<MovieDropdownsViewModel> GetMovieDropdownsValuesAsync();

        Task AddNewMovieAsync(MovieViewModel data);

        Task UpdateMovieAsync(MovieViewModel data);
    }
}
