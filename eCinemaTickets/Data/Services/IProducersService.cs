using eCinemaTickets.Data.Base;
using eCinemaTickets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCinemaTickets.Data.Services
{
    public interface IProducersService : IEntityBaseRepository<Producer>
    {
    }
}
