using dotnet_project.Models;
using dotnet_project.Services.MoMo;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_project.Controllers
{
    public class PaymentController : Controller
    {
        private IMomoService _momoService;
        public PaymentController(IMomoService momoService)
        {
            _momoService = momoService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentMoMo(OrderInfoModel model)
        {
            var response = await _momoService.CreatePaymentMoMo(model);
            if (string.IsNullOrEmpty(response.PayUrl))
            {
                // Xử lý lỗi, có thể trả về một thông báo hoặc chuyển hướng đến một trang lỗi
                return BadRequest("Payment URL is not available.");
            }
            return Redirect(response.PayUrl);
        }

        [HttpGet]
        public IActionResult PaymentCallBack()
        {
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            return View(response);
        }
    }
}
