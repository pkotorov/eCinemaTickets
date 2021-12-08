using eCinemaTickets.Data.Cart;
using eCinemaTickets.Data.Services;
using eCinemaTickets.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCinemaTickets.Controllers
{
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
            var userId = "";
            var orders = await this.ordersService.GetOrdersByUserIdAsync(userId);

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
            var userId = "";
            var userEmailAddress = "";

            await this.ordersService.StoreOrderAsync(items, userId, userEmailAddress);
            await this.shoppingCart.ClearShoppingCartAsync();

            return this.View("OrderCompleted");
        }

    }
}
