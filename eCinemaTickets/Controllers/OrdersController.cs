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

        public OrdersController(IMoviesService moviesService, ShoppingCart shoppingCart)
        {
            this.moviesService = moviesService;
            this.shoppingCart = shoppingCart;
        }

        public IActionResult Index()
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
    }
}
