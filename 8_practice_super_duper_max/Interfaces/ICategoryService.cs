using _8_practice_super_duper_max.Requests;
using Microsoft.AspNetCore.Mvc;

namespace _8_practice_super_duper_max.Interfaces
{
    public interface ICategoryService
    {
        Task<IActionResult> GetAllCategoriesAsync();
        Task<IActionResult> PostNewCategoryAsync(PostNewCategory postNewCategory);
        Task<IActionResult> PutCategoryAsync(int id, PutCategory putCategory);
        Task<IActionResult> DeleteCategoryAsync(int id, int user_id);
    }
}
