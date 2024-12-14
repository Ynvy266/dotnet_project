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
        public async Task<IActionResult> UpdateOrder(String ordercode, int status)
        {
            var order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.OrderCode == ordercode);
            if (order == null)
            {
                return NotFound();
            }
            order.Status = status;
            _dataContext.Update(order);
            if (status == 0)
            {
                var DetailOrder = await _dataContext.OrderDetails
                    .Include(od => od.Product)
                    .Where(od => od.OrderCode == order.OrderCode)
                    .Select(od => new
                    {
                        od.Quantity,
                        od.Product.Price,
                        od.Product.CapitalPrice
                    }).ToListAsync();
                //lay data thong ke dua vao ngay dat hang
                var statisticModel = await _dataContext.Statistic
                    .FirstOrDefaultAsync(s => s.DateCreated.Date == order.CreatedDate.Date);
                if (statisticModel != null)
                {
                    foreach (var orderDetail in DetailOrder)
                    {
                        statisticModel.Quantity += 1;
                        statisticModel.Sold += orderDetail.Quantity;
                        statisticModel.Revenue += orderDetail.Quantity * orderDetail.Price;
                        statisticModel.Profit += orderDetail.Price - orderDetail.CapitalPrice;

                    }
                    _dataContext.Update(statisticModel);
                }
                else
                {
                    int new_quantity = 0;
                    int new_sold = 0;
                    decimal new_profit = 0;
                    foreach (var orderDetail in DetailOrder)
                    {
                        new_quantity += 1;
                        new_sold += orderDetail.Quantity;
                        new_profit += orderDetail.Price - orderDetail.CapitalPrice;

                        statisticModel = new Models.StatisticModel
                        {
                            DateCreated = order.CreatedDate,
                            Quantity = new_quantity,
                            Sold = new_sold,
                            Revenue = orderDetail.Quantity * orderDetail.Price,
                            Profit = new_profit
                        };
                    }
                    _dataContext.Add(statisticModel);
                }
            }

            try
            {
                await _dataContext.SaveChangesAsync();
                return Ok(new { seccess = true, message = "Order status updated successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the order status.");
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
