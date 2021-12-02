using eCinemaTickets.Data;
using eCinemaTickets.Data.Services;
using eCinemaTickets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCinemaTickets.Controllers
{
    public class CinemasController : Controller
    {
        private readonly ICinemasService cinemasService;

        public CinemasController(ICinemasService cinemasService)
        {
            this.cinemasService = cinemasService;
        }

        public async Task<IActionResult> Index()
        {
            var cinemas = await this.cinemasService.GetAllAsync();

            return this.View(cinemas);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Logo,Name,Description")] Cinema cinema)
        {
            if (!ModelState.IsValid)
            {
                return this.View(cinema);
            }

            await this.cinemasService.AddAsync(cinema);

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var cinemaDetails = await this.cinemasService.GetByIdAsync(id);

            if (cinemaDetails == null)
            {
                return this.View(nameof(NotFound));
            }

            return this.View(cinemaDetails);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cinemaDetails = await this.cinemasService.GetByIdAsync(id);

            if (cinemaDetails == null)
            {
                return this.View(nameof(NotFound));
            }

            return this.View(cinemaDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Id,Logo,Name,Description")] Cinema cinema)
        {
            if (!ModelState.IsValid)
            {
                return this.View(cinema);
            }

            await this.cinemasService.UpdateAsync(cinema);

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cinemaDetails = await this.cinemasService.GetByIdAsync(id);

            if (cinemaDetails == null)
            {
                return this.View(nameof(NotFound));
            }

            return this.View(cinemaDetails);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cinemaDetails = await this.cinemasService.GetByIdAsync(id);

            if (cinemaDetails == null)
            {
                return this.View(nameof(NotFound));
            }

            await this.cinemasService.DeleteAsync(id);

            return this.RedirectToAction(nameof(Index));
        }
    }
}
