using eCinemaTickets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCinemaTickets.Data.Services
{
    public interface IActorsService
    {
        Task<IEnumerable<Actor>> GetAllAsync();

        Actor GetById(int id);

        Task AddAsync(Actor actor);

        Actor Update(int id, Actor newActor);

        void Delete(int id);
    }
}
