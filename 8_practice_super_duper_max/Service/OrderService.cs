using _8_practice_super_duper_max.DatabaseContext;
using _8_practice_super_duper_max.Interfaces;
using _8_practice_super_duper_max.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _8_practice_super_duper_max.Service
{
    public class OrderService : IOrderService
    {
        private readonly ContextDb _context;

        public OrderService(ContextDb context)
        {
            _context = context;
        }

        // просмотр всех заказов
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            var orders = await _context.Orders.ToListAsync();

            if (orders == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет заказов"
                });
            }

            return new OkObjectResult(new
            {
                data = new { orders = orders },
                status = true
            });
        }

        // изменение статуса заказа
        public async Task<IActionResult> PutOrderStatusAsync(int id, PutOrderStatus putOrderStatus)
        {
            if (id == 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Проблемы с Id"
                });
            }

            var existingOrder = await _context.Orders.FirstOrDefaultAsync(b => b.order_id == id);

            if (existingOrder == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет такого заказа с таким id"
                });
            }

            if (putOrderStatus.status_id == 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "У заказа должен быть статус"
                });
            }

            var existingStatus = await _context.Statuses.FirstOrDefaultAsync(s => s.status_id == putOrderStatus.status_id);

            if (existingStatus == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет такого статуса с таким id"
                });
            }

            existingOrder.status_id = putOrderStatus.status_id;

            await _context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                status = true
            });

        }
    }
}
