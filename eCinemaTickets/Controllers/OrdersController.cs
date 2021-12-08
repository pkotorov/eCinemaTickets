using eCinemaTickets.Data.Cart;
using eCinemaTickets.Data.Services;
using eCinemaTickets.Data.Static;
using eCinemaTickets.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eCinemaTickets.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IMoviesService moviesService;
        private readonly ShoppingCart shoppingCart;
        private readonly IOrdersService ordersService;

        public OrdersController(IMoviesService moviesService, ShoppingCart shoppingCart, IOrdersService ordersService)
        {
            this.moviesService = moviesService;
            this.shoppingCart = shoppingCart;
            this.ordersService = ordersService;
        }
        public async Task<IActionResult> Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = this.User.FindFirstValue(ClaimTypes.Role);
            var orders = await this.ordersService.GetOrdersByUserIdAndRoleAsync(userId, userRole);

            return this.View(orders);
        }

        public IActionResult ShoppingCart()
        {
            var items = this.shoppingCart.GetShoppingCartItems();
            this.shoppingCart.ShoppingCartItems = items;

            var response = new ShoppingCartViewModel()
            {
                ShoppingCart = this.shoppingCart,
                ShoppingCartTotal = this.shoppingCart.GetShoppingCartTotal(),
            };

            return this.View(response);
        }

        public async Task<RedirectToActionResult> AddItemToShoppingCart(int id)
        {
            var item = await this.moviesService.GetMovieByIdAsync(id);

            if (item != null)
            {
                this.shoppingCart.AddItemToCart(item);
            }

            return this.RedirectToAction(nameof(ShoppingCart));
        }

        public async Task<RedirectToActionResult> RemoveItemFromShoppingCart(int id)
        {
            var item = await this.moviesService.GetMovieByIdAsync(id);

            if (item != null)
            {
                this.shoppingCart.RemoveItemFromCart(item);
            }

            return this.RedirectToAction(nameof(ShoppingCart));
        }

        public async Task<IActionResult> CompleteOrder()
        {
            var items = this.shoppingCart.GetShoppingCartItems();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmailAddress = this.User.FindFirstValue(ClaimTypes.Email);

            await this.ordersService.StoreOrderAsync(items, userId, userEmailAddress);
            await this.shoppingCart.ClearShoppingCartAsync();

            return this.View("OrderCompleted");
        }

    }
}
