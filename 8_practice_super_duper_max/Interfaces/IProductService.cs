using _8_practice_super_duper_max.Requests;
using Microsoft.AspNetCore.Mvc;

namespace _8_practice_super_duper_max.Interfaces
{
    public interface IProductService
    {
        Task<IActionResult> GetAllProductsAsync(string filter_by_category, string sort_by_price, string sort_by_date ,int min_price, int max_price, bool in_stock);
        Task<IActionResult> PostNewProductAsync(PostNewProduct postNewPoduct);
        Task<IActionResult> PutProductAsync(int id, PutProduct putProduct);
        Task<IActionResult> DeleteProductAsync(int product_id, int user_id);
        Task<IActionResult> Top10ProductsAsync();
    }
}