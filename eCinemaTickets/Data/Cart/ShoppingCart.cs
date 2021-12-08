using eCinemaTickets.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCinemaTickets.Data.Cart
{
    public class ShoppingCart
    {
        private readonly AppDbContext context;

        public ShoppingCart(AppDbContext context)
        {
            this.context = context;
        }

        public string ShoppingCartId { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return this.ShoppingCartItems ?? (this.ShoppingCartItems = this.context.ShoppingCartItems
                .Where(x => x.ShoppingCartId == this.ShoppingCartId)
                .Include(n => n.Movie)
                .ToList());
        }

        public decimal GetShoppingCartTotal()
        {
            var total = this.context.ShoppingCartItems
                .Where(n => n.ShoppingCartId == this.ShoppingCartId)
                .Select(n => n.Movie.Price * n.Amount)
                .Sum();

            return total;
        }

        public void AddItemToCart(Movie movie)
        {
            var shoppingCartItem = this.context.ShoppingCartItems
                .FirstOrDefault(n => n.Movie.Id == movie.Id && n.ShoppingCartId == this.ShoppingCartId);

            if(shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem()
                {
                    ShoppingCartId = ShoppingCartId,
                    Movie = movie,
                    Amount = 1,
                };

                this.context.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }

            this.context.SaveChanges();
        }

        public void RemoveItemFromCart(Movie movie)
        {
            var shoppingCartItem = this.context.ShoppingCartItems
                 .FirstOrDefault(n => n.Movie.Id == movie.Id && n.ShoppingCartId == this.ShoppingCartId);

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                }
                else
                {
                    this.context.ShoppingCartItems.Remove(shoppingCartItem);
                }

                this.context.SaveChanges();
            }
        }

        public static ShoppingCart GetShoppingCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<AppDbContext>();

            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            session.SetString("CartId", cartId);

            return new ShoppingCart(context)
            {
                ShoppingCartId = cartId
            };
        }

        public async Task ClearShoppingCartAsync()
        {
            var items = await this.context.ShoppingCartItems
                .Where(x => x.ShoppingCartId == this.ShoppingCartId)
                .Include(n => n.Movie)
                .ToListAsync();

            this.context.ShoppingCartItems.RemoveRange(items);
            await this.context.SaveChangesAsync();

            this.ShoppingCartItems = new List<ShoppingCartItem>();
        }
    }
}
