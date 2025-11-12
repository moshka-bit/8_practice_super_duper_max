using _8_practice_super_duper_max.Requests;
using Microsoft.AspNetCore.Mvc;

namespace _8_practice_super_duper_max.Interfaces
{
    public interface IOrderService
    {
        Task<IActionResult> GetAllOrdersAsync();
        Task<IActionResult> PutOrderStatusAsync(int id, PutOrderStatus putOrderStatus);
    }
}
