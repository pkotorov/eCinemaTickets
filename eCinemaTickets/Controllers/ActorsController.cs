using eCinemaTickets.Data;
using eCinemaTickets.Data.Services;
using eCinemaTickets.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCinemaTickets.Controllers
{
    public class ActorsController : Controller
    {
        private readonly IActorsService actorsService;

        public ActorsController(IActorsService actorsService)
        {
            this.actorsService = actorsService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await this.actorsService.GetAllAsync();

            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("FullName,ProfilePictureUrl,Bio")]Actor actor)
        {
            if (!ModelState.IsValid)
            {
                return this.View(actor);
            }

            await this.actorsService.AddAsync(actor);

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var actorDetails = await actorsService.GetByIdAsync(id);

            if (actorDetails == null)
            {
                return this.View(nameof(NotFound));
            }

            return this.View(actorDetails);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var actorDetails = await actorsService.GetByIdAsync(id);

            if (actorDetails == null)
            {
                return this.View(nameof(NotFound));
            }

            return this.View(actorDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Id,FullName,ProfilePictureUrl,Bio")] Actor actor)
        {
            if (!ModelState.IsValid)
            {
                return this.View(actor);
            }

            await this.actorsService.UpdateAsync(actor);

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var actorDetails = await actorsService.GetByIdAsync(id);

            if (actorDetails == null)
            {
                return this.View(nameof(NotFound));
            }

            return this.View(actorDetails);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actorDetails = await actorsService.GetByIdAsync(id);

            if (actorDetails == null)
            {
                return this.View(nameof(NotFound));
            }

            await this.actorsService.DeleteAsync(id);

            return this.RedirectToAction(nameof(Index));
        }
    }
}
