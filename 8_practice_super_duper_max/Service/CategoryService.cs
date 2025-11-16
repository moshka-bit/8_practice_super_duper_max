using _8_practice_super_duper_max.DatabaseContext;
using _8_practice_super_duper_max.Interfaces;
using _8_practice_super_duper_max.Models;
using _8_practice_super_duper_max.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _8_practice_super_duper_max.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ContextDb _context;

        public CategoryService(ContextDb context)
        {
            _context = context;
        }

        // удаление категории
        public async Task<IActionResult> DeleteCategoryAsync(int id, int user_id)
        {
            if (id == 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Проблемы с Id"
                });
            }

            var existing_category = await _context.Categories.FirstOrDefaultAsync(p => p.category_id == id);

            if (existing_category == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет такой категории с таким id"
                });
            }

            _context.Categories.Remove(existing_category);

            var log = new LogUserAction()
            {
                created_at = DateTime.Now,
                user_id = user_id,
                action_type_id = 21 // CATEGORY DELETED
            };

            await _context.AddAsync(log);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                status = true
            });
        }

        // получение списка всех категорий
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();

            if (categories == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет категорий"
                });
            }

            return new OkObjectResult(new
            {
                data = new { categories = categories },
                status = true
            });
        }

        // добавление новой категории
        public async Task<IActionResult> PostNewCategoryAsync(PostNewCategory postNewCategory)
        {
            if (string.IsNullOrEmpty(postNewCategory.category_name))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Название категории не может быть пустым"
                });
            }

            var existing_category = await _context.Categories.FirstOrDefaultAsync(c => c.category_name.ToLower() == postNewCategory.category_name.ToLower());

            if (existing_category != null)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Уже есть категория с таким названием"
                });
            }

            var category = new Category()
            {
                category_name = postNewCategory.category_name
            };

            var log = new LogUserAction()
            {
                created_at = DateTime.Now,
                user_id = postNewCategory.user_id,
                action_type_id = 19 // CATEGORY_ADDED
            };

            await _context.AddAsync(log);
            await _context.AddAsync(category);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                status = true
            });
        }

        // изменение категории
        public async Task<IActionResult> PutCategoryAsync(int id, PutCategory putCategory)
        {
            if (id == 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Проблемы с Id"
                });
            }

            var existingCategory = await _context.Categories.FirstOrDefaultAsync(b => b.category_id == id);

            if (existingCategory == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет такой категории с таким id"
                });
            }

            if (string.IsNullOrEmpty(putCategory.category_name))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Название категории не может быть пустым"
                });
            }

            existingCategory.category_name = putCategory.category_name;

            var log = new LogUserAction()
            {
                created_at = DateTime.Now,
                user_id = putCategory.user_id,
                action_type_id = 20 // CATEGORY CHANGED
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
