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
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            return await _Service1.GetAllOrdersAsync();
        }

        [HttpPut]
        [Route("PutOrderStatus")]

        public async Task<IActionResult> PutOrderStatusAsync(int id, PutOrderStatus putOrderStatus)
        {
            return await _Service1.PutOrderStatusAsync(id, putOrderStatus);
        }
    }
}
