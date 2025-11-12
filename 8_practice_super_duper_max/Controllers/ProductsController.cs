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
        public async Task<IActionResult> GetAllProductsAsync()
        {
            return await _Service1.GetAllProductsAsync();
        }

        [HttpPost]
        [Route("PostNewProduct")]
        public async Task<IActionResult> PostNewProductAsync(PostNewProduct postNewProduct)
        {
            return await _Service1.PostNewProductAsync(postNewProduct);
        }

        [HttpPut]
        [Route("PutProduct")]

        public async Task<IActionResult> PutProductAsync(int id, PutProduct putProduct)
        {
            return await _Service1.PutProductAsync(id, putProduct);
        }

        [HttpDelete]
        [Route("DeleteProduct")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            return await _Service1.DeleteProductAsync(id);
        }
    }
}
