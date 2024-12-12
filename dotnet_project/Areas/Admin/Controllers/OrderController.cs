using dotnet_project.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Staff")]
    public class OrderController : Controller
    {
        private readonly DataContext _dataContext;

        public OrderController(DataContext context)
        {
            _dataContext = context;
        }

        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Orders.OrderByDescending(o => o.Id).ToListAsync());
        }

        [HttpGet]
        [Route("ViewOrder")]
        public async Task<IActionResult> ViewOrder(string ordercode)
        {
            var detailsOrder = await _dataContext.OrderDetails
                .Include(o => o.Product)
                .Where(o => o.OrderCode == ordercode)
                .ToListAsync();

            var order = _dataContext.Orders
                .Where(o => o.OrderCode == ordercode).First();
            //ViewBag.ShippingCost = order.ShippingCost;
            ViewBag.Status = order.Status;
            
            return View(detailsOrder);
        }

        [HttpPost]
        [Route("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(string ordercode, int status)
        {
            var order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.OrderCode == ordercode);
            if (order == null)
            {
                return NotFound();
            }
            order.Status = status;
            try
            {
                await _dataContext.SaveChangesAsync();
                return Ok(new { success = true, message = "Order status has been updated successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occured while updating the order status.");
            }
        }

        [HttpGet]
        [Route("PaymentMomoInfo")]
        public async Task<IActionResult> PaymentMomoInfo(string orderId)
        {
            var momoInfo = await _dataContext.MomoInfos.FirstOrDefaultAsync(m => m.OrderId == orderId);
            if (momoInfo == null)
            {
                return NotFound();
            }
            return View(momoInfo);
        }
    }
}
