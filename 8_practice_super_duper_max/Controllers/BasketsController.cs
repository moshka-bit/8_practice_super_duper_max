using _8_practice_super_duper_max.CustomAttributes;
using _8_practice_super_duper_max.Interfaces;
using _8_practice_super_duper_max.Requests;
using Microsoft.AspNetCore.Mvc;

namespace _8_practice_super_duper_max.Controllers
{
    public class BasketsController
    {
        private readonly IBasketService _Service1;

        public BasketsController(IBasketService service1)
        {
            _Service1 = service1;
        }

        [HttpPost]
        [Route("PostNewProductToBasket")]
        [RoleAuthorized([2])]
        public async Task<IActionResult> PostNewProductToBasketAsync(PostNewProductToBasket postNewProductToBasket)
        {
            return await _Service1.PostNewProductToBasketAsync(postNewProductToBasket);
        }

        [HttpDelete]
        [Route("DeleteProdcutFromBasket")]
        [RoleAuthorized([2])]
        public async Task<IActionResult> DeleteProdcutFromBasketAsync(DeleteProductFromBasket deleteProductFromBasket)
        {
            return await _Service1.DeleteProdcutFromBasketAsync(deleteProductFromBasket);
        }

    }
}
