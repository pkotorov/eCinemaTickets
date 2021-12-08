using eCinemaTickets.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCinemaTickets.Data.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly AppDbContext context;

        public OrdersService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string userId, string userRole)
        {
            var orders = await this.context.Orders
                .Include(n => n.OrderItems)
                .ThenInclude(n => n.Movie)
                .Include(n => n.User)
                .ToListAsync();

            if (userRole != "Admin")
            {
                orders = orders.Where(n => n.UserId == userId).ToList();
            }

            return orders;
        }

        public async Task StoreOrderAsync(List<ShoppingCartItem> shoppingCartItems, string userId, string userEmailAddress)
        {
            var order = new Order()
            {
                UserId = userId,
                Email = userEmailAddress,
            };

            await this.context.Orders.AddAsync(order);
            await this.context.SaveChangesAsync();

            foreach (var item in shoppingCartItems)
            {
                var orderItem = new OrderItem()
                {
                    Amount = item.Amount,
                    MovieId = item.MovieId,
                    OrderId = order.Id,
                    Price = item.Movie.Price,
                };

                await this.context.OrderItems.AddAsync(orderItem);
            }

            await this.context.SaveChangesAsync();
        }
    }
}
