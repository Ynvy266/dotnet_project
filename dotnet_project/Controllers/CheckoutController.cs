using dotnet_project.Models;
using dotnet_project.Models.ViewModels;
using dotnet_project.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace dotnet_project.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DataContext _dataContext;

        public CheckoutController(DataContext context)
        {
            _dataContext = context;
        }

        public async Task<IActionResult> Checkout()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var orderCode = Guid.NewGuid().ToString();
                var orderItem = new OrderModel();
                orderItem.OrderCode = orderCode;
                orderItem.UserName = userEmail;
                orderItem.Status = 1; //new order
                orderItem.CreatedDate = DateTime.Now;
                _dataContext.Add(orderItem);
                _dataContext.SaveChanges();
                List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
                foreach (var cartItem in cart)
                {
                    var orderdetails = new OrderDetailsModel();
                    orderdetails.UserName = userEmail;
                    orderdetails.OrderCode = orderCode;
                    orderdetails.ProductId = cartItem.ProductId;
                    orderdetails.Price = cartItem.Price;
                    orderdetails.Quantity = cartItem.Quantity;
                    _dataContext.Add(orderdetails);
                    _dataContext.SaveChanges();
                }
                HttpContext.Session.Remove("Cart");
                TempData["success"] = "Checkout successful! Please wait for your order to be approved.";
                return RedirectToAction("Index", "Cart");
            }
            return View();
        }
    }
}
