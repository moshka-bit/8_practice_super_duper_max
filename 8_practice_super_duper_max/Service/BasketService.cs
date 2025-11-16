using _8_practice_super_duper_max.DatabaseContext;
using _8_practice_super_duper_max.Interfaces;
using _8_practice_super_duper_max.Models;
using _8_practice_super_duper_max.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _8_practice_super_duper_max.Service
{
    public class BasketService : IBasketService
    {
        private readonly ContextDb _context;

        public BasketService(ContextDb context)
        {
            _context = context;
        }

        // удаление товара из корзины
        public async Task<IActionResult> DeleteProdcutFromBasketAsync(DeleteProductFromBasket deleteProductFromBasket)
        {
            if (deleteProductFromBasket.product_id == 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Отсутствует product_id"
                });
            }

            var existing_product = await _context.Products.FirstOrDefaultAsync(p => p.product_id == deleteProductFromBasket.product_id);

            if (existing_product == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет такого продукта с таким id"
                });
            }

            if (deleteProductFromBasket.user_id == 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Отсутствует user_id"
                });
            }

            var existing_user = await _context.Users.FirstOrDefaultAsync(p => p.user_id == deleteProductFromBasket.user_id);

            if (existing_user == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет такого пользователя с таким id"
                });
            }

            var existing_basket = await _context.Baskets.FirstOrDefaultAsync(b => b.user_id == deleteProductFromBasket.user_id && b.order_id == null);

            if (existing_basket == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "У пользователя нет заказанных товаров"
                });
            }

            var product_in_basket_items = await _context.BasketItems.FirstOrDefaultAsync(bi => bi.product_id == deleteProductFromBasket.product_id && bi.basket_id == existing_basket.basket_id);

            if (product_in_basket_items == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Такого товара нет в корзине"
                });
            }

            _context.BasketItems.Remove(product_in_basket_items);
            existing_basket.result_price -= Math.Round(product_in_basket_items.Product.price * product_in_basket_items.quantity, 2);

            existing_product.stock += product_in_basket_items.quantity;

            var log = new LogUserAction()
            {
                created_at = DateTime.Now,
                user_id = deleteProductFromBasket.user_id,
                action_type_id = 5 // ITEM_REMOVED
            };

            await _context.AddAsync(log);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                status = true
            });

        }

        // добавление товара в корзину
        public async Task<IActionResult> PostNewProductToBasketAsync(PostNewProductToBasket postNewProductToBasket)
        {
            if(postNewProductToBasket.user_id == 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "У корзины должен быть user_id"
                });
            }

            var existing_user = await _context.Users.FirstOrDefaultAsync(u => u.user_id == postNewProductToBasket.user_id);

            if (existing_user == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет такого пользователя с таким id"
                });
            }

            if(postNewProductToBasket.quantity <= 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Неккоректное значение у quantity"
                });
            }

            if (postNewProductToBasket.product_id == 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "У корзины должен быть product_id"
                });
            }

            var existing_product = await _context.Products.FirstOrDefaultAsync(u => u.product_id == postNewProductToBasket.product_id);

            if (existing_product == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет такого товара с таким id"
                });
            }

            if (existing_product.stock == 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Товара нет в наличии"
                });
            }

            var existing_basket = await _context.Baskets.FirstOrDefaultAsync(b => b.user_id == postNewProductToBasket.user_id && b.order_id == null);

            if (existing_basket == null)
            {
                var basket = new Basket()
                {
                    result_price = 0,
                    user_id = postNewProductToBasket.user_id,
                    order_id = null
                };
                await _context.AddAsync(basket);
            }
            await _context.SaveChangesAsync();

            var our_basket = await _context.Baskets.FirstOrDefaultAsync(b => b.user_id == postNewProductToBasket.user_id && b.order_id == null);

            var product_in_basket_items = await _context.BasketItems.FirstOrDefaultAsync(bi => bi.product_id == postNewProductToBasket.product_id && bi.basket_id == our_basket.basket_id);

            if (product_in_basket_items == null)
            {
                var basket_item = new BasketItems()
                {
                    quantity = postNewProductToBasket.quantity,
                    basket_id = our_basket.basket_id,
                    product_id = postNewProductToBasket.product_id
                };
                await _context.AddAsync(basket_item);
            }
            else
            {
                product_in_basket_items.quantity += postNewProductToBasket.quantity;
            }

            our_basket.result_price += Math.Round(existing_product.price * postNewProductToBasket.quantity, 2);
            if (our_basket.result_price > 0 && our_basket.result_price < 1)
            {
                our_basket.result_price = 0.00;
            }

            existing_product.stock -= postNewProductToBasket.quantity;

            var log = new LogUserAction()
            {
                created_at = DateTime.Now,
                user_id = postNewProductToBasket.user_id,
                action_type_id = 4 // ITEM_ADDED
            };

            await _context.AddAsync(log);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                status = true
            });
        }
    }
}
