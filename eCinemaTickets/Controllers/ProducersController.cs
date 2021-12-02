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
    public class ProducersController : Controller
    {
        private readonly IProducersService producersService;

        public ProducersController(IProducersService producersService)
        {
            this.producersService = producersService;
        }

        public async Task<IActionResult> Index()
        {
            var producers = await this.producersService.GetAllAsync();

            return this.View(producers);
        }

        public async Task<IActionResult> Details(int id)
        {
            var producerDetails = await this.producersService.GetByIdAsync(id);

            if (producerDetails == null)
            {
                return this.View(nameof(NotFound));
            }

            return this.View(producerDetails);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("ProfilePictureUrl,FullName,Bio")]Producer producer)
        {
            if (!ModelState.IsValid)
            {
                return this.View(producer);
            }

            await this.producersService.AddAsync(producer);

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var producerDetails = await this.producersService.GetByIdAsync(id);

            if (producerDetails == null)
            {
                return this.View(nameof(NotFound));
            }

            return this.View(producerDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Id,FullName,ProfilePictureUrl,Bio")] Producer producer)
        {
            if (!ModelState.IsValid)
            {
                return this.View(producer);
            }

            await this.producersService.UpdateAsync(producer);

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var producerDetails = await this.producersService.GetByIdAsync(id);

            if (producerDetails == null)
            {
                return this.View(nameof(NotFound));
            }

            return this.View(producerDetails);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producerDetails = await this.producersService.GetByIdAsync(id);

            if (producerDetails == null)
            {
                return this.View(nameof(NotFound));
            }

            await this.producersService.DeleteAsync(id);

            return this.RedirectToAction(nameof(Index));
        }
    }
}
