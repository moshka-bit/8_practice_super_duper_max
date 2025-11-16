using _8_practice_super_duper_max.CustomAttributes;
using _8_practice_super_duper_max.Interfaces;
using _8_practice_super_duper_max.Requests;
using Microsoft.AspNetCore.Mvc;

namespace _8_practice_super_duper_max.Controllers
{
    public class OrdersController
    {
        private readonly IOrderService _Service1;

        public OrdersController(IOrderService service1)
        {
            _Service1 = service1;
        }

        [HttpGet]
        [Route("GetAllOrders")]
        [RoleAuthorized([1, 3])]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            return await _Service1.GetAllOrdersAsync();
        }

        [HttpPut]
        [Route("PutOrderStatus")]
        [RoleAuthorized([1, 3])]
        public async Task<IActionResult> PutOrderStatusAsync(int id, PutOrderStatus putOrderStatus)
        {
            return await _Service1.PutOrderStatusAsync(id, putOrderStatus);
        }

        [HttpPost]
        [Route("PostNewOrder")]
        [RoleAuthorized([2])]
        public async Task<IActionResult> PostNewOrderAsync(PostNewOrder postNewOrder)
        {
            return await _Service1.PostNewOrderAsync(postNewOrder);
        }

        [HttpGet]
        [Route("GetOrdersByUserId")]
        [RoleAuthorized([2])]
        public async Task<IActionResult> GetOrdersByUserIdAsync(int userId)
        {
            return await _Service1.GetOrdersByUserIdAsync(userId);
        }

        [HttpPut]
        [Route("PutOrderStatusWhenCancelled")]
        [RoleAuthorized([2])]
        public async Task<IActionResult> PutOrderStatusWhenCancelledAsync(int order_id)
        {
            return await _Service1.PutOrderStatusWhenCancelledAsync(order_id);
        }

        [HttpGet]
        [Route("DailyReportSales")]
        [RoleAuthorized([3])]
        public async Task<IActionResult> DailyReportSalesAsync()
        {
            return await _Service1.DailyReportSalesAsync();
        }

        [HttpGet]
        [Route("WeeklyReportsSales")]
        [RoleAuthorized([3])]
        public async Task<IActionResult> WeeklyReportsSalesAsync()
        {
            return await _Service1.WeeklyReportsSalesAsync();
        }
    }
}
