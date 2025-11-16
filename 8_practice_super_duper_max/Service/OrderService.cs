using _8_practice_super_duper_max.DatabaseContext;
using _8_practice_super_duper_max.Interfaces;
using _8_practice_super_duper_max.Models;
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

        // ежедневный отчёт по продажам
        public async Task<IActionResult> DailyReportSalesAsync()
        {
            var orders = await _context.Orders.Where(o => o.status_id == 4 && o.order_date == DateOnly.FromDateTime(DateTime.Now)).ToListAsync();

            if (orders.Count == 0)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Сегодня нет продаж"
                });
            }

            var result = new
            {
                date = DateOnly.FromDateTime(DateTime.Now),
                total_orders = orders.Count,
                total_money = orders.Sum(o => o.total_amount)
            };


            return new OkObjectResult(new
            {
                data = new { result = result },
                status = true
            });
        }

        // просмотр всех заказов админом
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

        // просмотр заказов пользователем
        public async Task<IActionResult> GetOrdersByUserIdAsync(int userId)
        {
            if (userId == 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Проблемы с Id"
                });
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.user_id == userId);

            if (existingUser == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет такого пользователя с таким id"
                });
            }

            var orders = _context.Orders.Where(o => o.user_id == userId);

            if (orders.Count() == 0)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "У этого пользователя нет заказов"
                });
            }


            return new OkObjectResult(new
            {
                data = new { orders = orders.Include(o => o.Status) },
                status = true
            });
        }

        // оформление заказа
        public async Task<IActionResult> PostNewOrderAsync(PostNewOrder postNewOrder)
        {
            if (postNewOrder.delivery_type_id == 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Необходимо указать тип доставки"
                });
            }

            var existing_delivery_type = await _context.DeliveryTypes.FirstOrDefaultAsync(dt => dt.delivery_type_id == postNewOrder.delivery_type_id);

            if (existing_delivery_type == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет такого типа доставки с таким id"
                });
            }

            if (postNewOrder.payment_type_id == 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Необходимо указать тип оплаты"
                });
            }

            var existing_payment_type = await _context.PaymentTypes.FirstOrDefaultAsync(dt => dt.payment_type_id == postNewOrder.payment_type_id);

            if (existing_payment_type == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет такого типа оплаты с таким id"
                });
            }

            if (postNewOrder.user_id == 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Необходимо указать пользователя"
                });
            }

            var existing_user = await _context.Users.FirstOrDefaultAsync(dt => dt.user_id == postNewOrder.user_id);

            if (existing_user == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет такого пользователя с таким id"
                });
            }

            var existing_basket = await _context.Baskets.FirstOrDefaultAsync(b => b.user_id == postNewOrder.user_id && b.order_id == null);

            if (existing_basket == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "В корзине нет товаров, чтобы оформить заказ"
                });
            }

            var existing_items_basket = await _context.BasketItems.FirstOrDefaultAsync(bi => bi.basket_id == existing_basket.basket_id);

            if (existing_items_basket == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "В корзине нет товаров, чтобы оформить заказ"
                });
            }

            var order = new Order()
            {
                order_date = DateOnly.FromDateTime(DateTime.Now),
                total_amount = existing_basket.result_price,
                status_id = 1,
                user_id = postNewOrder.user_id,
                delivery_type_id = postNewOrder.delivery_type_id,
                payment_type_id = postNewOrder.payment_type_id,
            };

            var log = new LogUserAction()
            {
                created_at = DateTime.Now,
                user_id = postNewOrder.user_id,
                action_type_id = 8 // ORDER_CREATED
            };

            await _context.AddAsync(log);
            await _context.AddAsync(order);
            await _context.SaveChangesAsync();

            existing_basket.order_id = order.order_id;

            var basket = new Basket()
            {
                result_price = 0,
                user_id = postNewOrder.user_id,
                order_id = null
            };

            await _context.AddAsync(basket);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new
            {
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

            var log = new LogUserAction()
            {
                created_at = DateTime.Now,
                user_id = putOrderStatus.user_id,
                action_type_id = 11 // ORDER_STATUS_CHANGED
            };

            await _context.AddAsync(log);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                status = true
            });

        }

        // отмена заказа
        public async Task<IActionResult> PutOrderStatusWhenCancelledAsync(int order_id)
        {
            if (order_id == 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Отсутствует order_id"
                });
            }

            var existing_order = await _context.Orders.FirstOrDefaultAsync(p => p.order_id == order_id);

            if (existing_order == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет такого заказа с таким id"
                });
            }

            if (existing_order.status_id != 1)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Неподходящий статус заказа для отмены"
                });
            }

            existing_order.status_id = 5; // статус cancelled

            var log = new LogUserAction()
            {
                created_at = DateTime.Now,
                user_id = existing_order.user_id,
                action_type_id = 10 // ORDER_CANCELLED
            };

            await _context.AddAsync(log);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                status = true
            });

        }

        // еженедельный отчёт по продажам
        public async Task<IActionResult> WeeklyReportsSalesAsync()
        {
            var orders = await _context.Orders.Where(o => o.status_id == 4 && o.order_date >= DateOnly.FromDateTime(DateTime.Now).AddDays(-6) && o.order_date <= DateOnly.FromDateTime(DateTime.Now)).ToListAsync();

            if (orders.Count == 0)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "На этой неделе не было продаж"
                });
            }

            var result = new
            {
                start_date = DateOnly.FromDateTime(DateTime.Now).AddDays(-6),
                end_date = DateOnly.FromDateTime(DateTime.Now),
                total_orders = orders.Count,
                total_money = orders.Sum(o => o.total_amount)
            };


            return new OkObjectResult(new
            {
                data = new { result = result },
                status = true
            });
        }
    }
}
