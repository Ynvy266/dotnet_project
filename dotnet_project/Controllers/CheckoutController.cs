using dotnet_project.Areas.Admin.Repository;
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
        private readonly IEmailSender _emailSender;

        public CheckoutController(DataContext context, IEmailSender emailSender)
        {
            _dataContext = context;
            _emailSender = emailSender;
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
                //Send email to confirm checkout
                var receiver = "nguyenyenvy266@gmail.com";
                var subject = "Order placed successfully!";
                var message = "Order placed successfully!";

                await _emailSender.SendEmailAsync(receiver, subject, message);
                TempData["success"] = "Checkout successful! Please wait for your order to be approved.";
                return RedirectToAction("Index", "Cart");
            }
            return View();
        }
    }
}