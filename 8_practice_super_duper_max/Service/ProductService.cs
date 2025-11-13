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

        // получение списка всех продуктов
        public async Task<IActionResult> GetAllProductsAsync(
            string filter_by_category,
            string sort_by_price,
            string sort_by_date,  // новый параметр для сортировки по дате
            int min_price,
            int max_price,
            bool in_stock)
        {
            List<Product> products;

            if (string.IsNullOrEmpty(filter_by_category))
            {
                products = await _context.Products
                    .Include(p => p.Category)
                    .ToListAsync();
            }
            else
            {
                products = await _context.Products
                    .Include(p => p.Category)
                    .Where(p => p.Category.category_name.Contains(filter_by_category))
                    .ToListAsync();
            }

            if (min_price < 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Минимальная цена не может быть отрицательной"
                });
            }

            if (max_price < 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Максимальная цена не может быть отрицательной"
                });
            }

            if (max_price > 0 && min_price > max_price)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Минимальная цена не может быть больше максимальной"
                });
            }

            if (min_price > 0)
            {
                products = products.Where(p => p.price >= min_price).ToList();
            }

            if (max_price > 0)
            {
                products = products.Where(p => p.price <= max_price).ToList();
            }

            if (in_stock == true)
            {
                // Только товары в наличии (stock > 0)
                products = products.Where(p => p.stock > 0).ToList();
            }
            else
            {
                // Только товары не в наличии (stock == 0)
                products = products.Where(p => p.stock == 0).ToList();
            }

            // Сначала применяем сортировку по дате (если указана)
            if (!string.IsNullOrEmpty(sort_by_date))
            {
                if (sort_by_date.ToLower() == "desc")
                {
                    products = products.OrderByDescending(p => p.created_at).ToList();
                }
                else if (sort_by_date.ToLower() == "asc")
                {
                    products = products.OrderBy(p => p.created_at).ToList();
                }
            }

            // Затем применяем сортировку по цене (если указана)
            if (!string.IsNullOrEmpty(sort_by_price))
            {
                if (sort_by_price.ToLower() == "desc")
                {
                    products = products.OrderByDescending(p => p.price).ToList();
                }
                else if (sort_by_price.ToLower() == "asc")
                {
                    products = products.OrderBy(p => p.price).ToList();
                }
            }

            // Если не указана ни одна сортировка, сортируем по имени по умолчанию
            if (string.IsNullOrEmpty(sort_by_date) && string.IsNullOrEmpty(sort_by_price))
            {
                products = products.OrderBy(p => p.product_name).ToList();
            }

            if (products.Count == 0)
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
