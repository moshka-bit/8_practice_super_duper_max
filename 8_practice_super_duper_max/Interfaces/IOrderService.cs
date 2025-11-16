using _8_practice_super_duper_max.Requests;
using Microsoft.AspNetCore.Mvc;

namespace _8_practice_super_duper_max.Interfaces
{
    public interface IOrderService
    {
        Task<IActionResult> GetAllOrdersAsync();
        Task<IActionResult> PutOrderStatusAsync(int id, PutOrderStatus putOrderStatus);
        Task<IActionResult> PostNewOrderAsync(PostNewOrder postNewOrder);
        Task<IActionResult> GetOrdersByUserIdAsync(int userId);
        Task<IActionResult> PutOrderStatusWhenCancelledAsync(int order_id);
        Task<IActionResult> DailyReportSalesAsync();
        Task<IActionResult> WeeklyReportsSalesAsync();
    }
}
