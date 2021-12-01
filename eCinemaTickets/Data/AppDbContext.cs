using eCinemaTickets.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCinemaTickets.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }
        public DbSet<Actor> Actors { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Cinema> Cinemas { get; set; }

        public DbSet<ActorMovie> ActorsMovies { get; set; }

        public DbSet<Producer> Producers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActorMovie>().HasKey(am => new
            {
                am.ActorId,
                am.MovieId
            });

            modelBuilder.Entity<ActorMovie>()
                .HasOne(m => m.Movie)
                .WithMany(am => am.ActorsMovies)
                .HasForeignKey(m => m.MovieId);
            modelBuilder.Entity<ActorMovie>()
                .HasOne(m => m.Actor)
                .WithMany(am => am.ActorsMovies)
                .HasForeignKey(m => m.ActorId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
