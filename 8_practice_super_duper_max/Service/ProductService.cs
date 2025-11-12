using _8_practice_super_duper_max.DatabaseContext;
using _8_practice_super_duper_max.Interfaces;
using _8_practice_super_duper_max.Models;
using _8_practice_super_duper_max.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace _8_practice_super_duper_max.Service
{
    public class ProductService : IProductService
    {
        private readonly ContextDb _context;

        public ProductService(ContextDb context)
        {
            _context = context;
        }

        // удаление продукта
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            if (id == 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Проблемы с Id"
                });
            }

            var existing_product = await _context.Products.FirstOrDefaultAsync(p => p.product_id == id);

            if (existing_product == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет такого продукта с таким id"
                });
            }

            _context.Products.Remove(existing_product);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                status = true
            });
        }

        // получить список всех продуктов
        public async Task<IActionResult> GetAllProductsAsync()
        {
            var products = await _context.Products.ToListAsync();

            if (products == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет продуктов"
                });
            }

            return new OkObjectResult(new
            {
                data = new { products = products },
                status = true
            });
        }

        // добавление нового продукта
        public async Task<IActionResult> PostNewProductAsync(PostNewProduct postNewPoduct)
        {
            if (string.IsNullOrEmpty(postNewPoduct.product_name))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Название продукта не может быть пустым"
                });
            }

            if (string.IsNullOrEmpty(postNewPoduct.description))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Описание продукта не может быть пустым"
                });
            }

            if (postNewPoduct.price <= 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "У продукта должна быть цена, причём положительная"
                });
            }

            if (postNewPoduct.stock <= 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "У продукта должно быть кол-во на складе, причём это положительное число"
                });
            }

            if (postNewPoduct.category_id == 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "У продукта должна быть категория"
                });
            }

            var existing_category = await _context.Categories.FirstOrDefaultAsync(c => c.category_id ==  postNewPoduct.category_id);

            if (existing_category == null)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Нет такой категории с таким id"
                });
            }

            var existing_product = await _context.Products.FirstOrDefaultAsync(p => p.product_name.ToLower() == postNewPoduct.product_name.ToLower());

            if (existing_product != null)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Товар с таким названием уже существует"
                });
            }

            var product = new Product()
            {
                product_name = postNewPoduct.product_name,
                description = postNewPoduct.description,
                price = postNewPoduct.price,
                created_at = DateOnly.FromDateTime(DateTime.Now),
                is_active = postNewPoduct.is_active,
                stock = postNewPoduct.stock,
                category_id = postNewPoduct.category_id
            };

            await _context.AddAsync(product);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                status = true
            });
        }

        // изменение продукта
        public async Task<IActionResult> PutProductAsync(int id, PutProduct putProduct)
        {
            if (id == 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Проблемы с Id"
                });
            }

            var existingProduct = await _context.Products.FirstOrDefaultAsync(b => b.product_id == id);

            if (existingProduct == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет такого продукта с таким id"
                });
            }

            if (string.IsNullOrEmpty(putProduct.product_name))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Название продукта не может быть пустым"
                });
            }

            if (string.IsNullOrEmpty(putProduct.description))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Описание продукта не может быть пустым"
                });
            }

            if (putProduct.price <= 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "У продукта должна быть цена, причём положительная"
                });
            }

            if (putProduct.stock <= 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "У продукта должно быть кол-во на складе, причём это положительное число"
                });
            }

            if (putProduct.category_id == 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "У продукта должна быть категория"
                });
            }

            var existing_category = await _context.Categories.FirstOrDefaultAsync(c => c.category_id == putProduct.category_id);

            if (existing_category == null)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Нет такой категории с таким id"
                });
            }

            existingProduct.product_name = putProduct.product_name;
            existingProduct.description = putProduct.description;
            existingProduct.price = putProduct.price;
            existingProduct.stock = putProduct.stock;
            existingProduct.is_active = putProduct.is_active;
            existingProduct.category_id = putProduct.category_id;

            await _context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                status = true
            });
        }
    }
}
