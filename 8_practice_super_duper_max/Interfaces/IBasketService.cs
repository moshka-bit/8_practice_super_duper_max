using _8_practice_super_duper_max.Requests;
using Microsoft.AspNetCore.Mvc;

namespace _8_practice_super_duper_max.Interfaces
{
    public interface IBasketService
    {
        Task<IActionResult> PostNewProductToBasketAsync(PostNewProductToBasket postNewProductToBasket);
        Task<IActionResult> DeleteProdcutFromBasketAsync(DeleteProductFromBasket deleteProductFromBasket);
    }
}
