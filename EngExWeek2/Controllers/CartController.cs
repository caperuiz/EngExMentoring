using CartingService.BLL.CartingService.BLL;
using CartingService.BLL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EngExWeek2.Controllers
{
    /// <summary>
    /// Controller for managing shopping carts.
    /// </summary>
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;
        private readonly ICartService _cartService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="cartService">The cart service.</param>
        public CartController(ILogger<CartController> logger, ICartService cartService)
        {
            _logger = logger;
            _cartService = cartService;
        }

        /// <summary>
        /// Get information about a specific cart by its ID.
        /// </summary>
        /// <param name="id">The ID of the cart.</param>
        /// <returns>The cart information.</returns>
        [HttpGet("GetCartInfo/{id}")]
        [ProducesResponseType(200, Type = typeof(Cart))]
        [ProducesResponseType(400, Type = typeof(string))]
        public IActionResult GetCart(Guid id)
        {
            try
            {
                var cart = _cartService.GetCart(id);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get a list of all carts.
        /// </summary>
        /// <returns>The list of carts.</returns>
        [HttpGet("GetAllCarts")]
        [ProducesResponseType(200, Type = typeof(List<Cart>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public IActionResult GetAllCarts()
        {
            try
            {
                var carts = _cartService.GetAllCarts();
                return Ok(carts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Add an item to a cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart.</param>
        /// <param name="item">The item to add to the cart.</param>
        /// <returns>OK if the item was added successfully; otherwise, a 409 Conflict or 400 Bad Request in case of an error.</returns>
        [HttpPost("AddItemToCart")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(409, Type = typeof(string))]
        public IActionResult AddItemToCart(Guid cartId, AddCartItem item)
        {
            try
            {
                _cartService.AddItemToCart(cartId, item);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Remove an item from a cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart.</param>
        /// <param name="itemId">The ID of the item to remove.</param>
        /// <returns>OK if the item was removed successfully; otherwise, a 404 Not Found or 400 Bad Request in case of an error.</returns>
        [HttpDelete("RemoveItemFromCart")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public IActionResult RemoveItemFromCart(Guid cartId, Guid itemId)
        {
            try
            {
                _cartService.RemoveItemFromCart(cartId, itemId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
