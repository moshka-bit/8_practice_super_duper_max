using _8_practice_super_duper_max.CustomAttributes;
using _8_practice_super_duper_max.Interfaces;
using _8_practice_super_duper_max.Requests;
using Microsoft.AspNetCore.Mvc;

namespace _8_practice_super_duper_max.Controllers
{
    public class ProductsController
    {
        private readonly IProductService _Service1;

        public ProductsController(IProductService service1)
        {
            _Service1 = service1;
        }

        [HttpGet]
        [Route("GetAllProducts")]
        public async Task<IActionResult> GetAllProductsAsync(string filter_by_category, string sort_by_price, string sort_by_date ,int min_price, int max_price, bool in_stock)
        {
            return await _Service1.GetAllProductsAsync(filter_by_category, sort_by_price, sort_by_date, min_price, max_price, in_stock);
        }

        [HttpPost]
        [Route("PostNewProduct")]
        [RoleAuthorized([1, 3])]
        public async Task<IActionResult> PostNewProductAsync(PostNewProduct postNewProduct)
        {
            return await _Service1.PostNewProductAsync(postNewProduct);
        }

        [HttpPut]
        [Route("PutProduct")]
        [RoleAuthorized([1, 3])]
        public async Task<IActionResult> PutProductAsync(int id, PutProduct putProduct)
        {
            return await _Service1.PutProductAsync(id, putProduct);
        }

        [HttpDelete]
        [Route("DeleteProduct")]
        [RoleAuthorized([1, 3])]    
        public async Task<IActionResult> DeleteProductAsync(int product_id, int user_id)
        {
            return await _Service1.DeleteProductAsync(product_id, user_id);
        }

        [HttpGet]
        [Route("Top10Products")]
        [RoleAuthorized([3])]
        public async Task<IActionResult> Top10ProductsAsync()
        {
            return await _Service1.Top10ProductsAsync();
        }
    }
}
