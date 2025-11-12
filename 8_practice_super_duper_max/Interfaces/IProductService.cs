using _8_practice_super_duper_max.Requests;
using Microsoft.AspNetCore.Mvc;

namespace _8_practice_super_duper_max.Interfaces
{
    public interface IProductService
    {
        Task<IActionResult> GetAllProductsAsync();
        Task<IActionResult> PostNewProductAsync(PostNewProduct postNewPoduct);
        Task<IActionResult> PutProductAsync(int id, PutProduct putProduct);
        Task<IActionResult> DeleteProductAsync(int id);
    }
}