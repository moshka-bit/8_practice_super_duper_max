using _8_practice_super_duper_max.CustomAttributes;
using _8_practice_super_duper_max.Interfaces;
using _8_practice_super_duper_max.Requests;
using Microsoft.AspNetCore.Mvc;

namespace _8_practice_super_duper_max.Controllers
{
    public class CategoriesController
    {
        private readonly ICategoryService _Service1;

        public CategoriesController(ICategoryService service1)
        {
            _Service1 = service1;
        }

        [HttpGet]
        [Route("GetAllCategories")]
        [RoleAuthorized([1])]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            return await _Service1.GetAllCategoriesAsync();
        }

        [HttpPost]
        [Route("PostNewCategory")]
        [RoleAuthorized([1])]
        public async Task<IActionResult> PostNewCategoryAsync(PostNewCategory postNewCategory)
        {
            return await _Service1.PostNewCategoryAsync(postNewCategory);
        }

        [HttpPut]
        [Route("PutCategory")]
        [RoleAuthorized([1])]
        public async Task<IActionResult> PutCategoryAsync(int id, PutCategory putCategory)
        {
            return await _Service1.PutCategoryAsync(id, putCategory);
        }

        [HttpDelete]
        [Route("DeleteCategory")]
        [RoleAuthorized([1])]
        public async Task<IActionResult> DeleteCategoryAsync(int id, int user_id)
        {
            return await _Service1.DeleteCategoryAsync(id, user_id);
        }
    }
}
