using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Store.Commons.Exeptions;
using Store.Services.Cart;
using Store.Web.Extensions;
using Store.Web.Models;

namespace Store.Web.Controllers
{
    [Route("[controller]")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize]
        public async Task<IActionResult> IndexAsync()
        {
            var cart = await _cartService.GetOrCreateCartAsync(User.GetId());

            if (cart is null)
            {
                return RedirectToAction("All", "Products");
            }

            return View(cart);
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                var userId = User.GetId();

                if (userId is null)
                {
                    return Unauthorized();
                }

                var cart = await _cartService.GetOrCreateCartAsync(userId);
                return Ok(cart);
            }
            catch (UserNotificationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            try
            {
                var userId = User.GetId();

                if (userId is null)
                {
                    return Unauthorized();
                }

                await _cartService.AddToCartAsync(userId, request.ProductId, request.Quantity);
                return Ok();
            }
            catch (UserNotificationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("remove/{productId}")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            try
            {
                if (productId <= 0) {
                    return BadRequest();
                }

                await _cartService.RemoveFromCartAsync(User.GetId(), productId);
                return Ok();
            }
            catch (UserNotificationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                await _cartService.ClearCartAsync(User.GetId());
                return Ok();
            }
            catch (UserNotificationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("total")]
        public async Task<IActionResult> GetTotal(string userId)
        {
            try
            {
                var total = await _cartService.GetTotalAsync(userId);
                return Ok(new { total });
            }
            catch (UserNotificationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}