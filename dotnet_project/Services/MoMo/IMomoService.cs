using dotnet_project.Models;
using dotnet_project.Models.MoMo;

namespace dotnet_project.Services.MoMo
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentMoMo(OrderInfoModel model);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
    }
}
