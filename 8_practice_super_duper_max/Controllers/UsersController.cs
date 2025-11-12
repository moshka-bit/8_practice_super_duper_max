using _8_practice_super_duper_max.Interfaces;
using _8_practice_super_duper_max.Requests;
using Microsoft.AspNetCore.Mvc;

namespace _8_practice_super_duper_max.Controllers
{
    public class UsersController
    {
        private readonly IUserService _Service1;

        public UsersController(IUserService service1)
        {
            _Service1 = service1;
        }

        [HttpGet]
        [Route("GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployeesAsync()
        {
            return await _Service1.GetAllEmployeesAsync();
        }

        [HttpGet]
        [Route("GetAllCustomers")]
        public async Task<IActionResult> GetAllCustomersAsync()
        {
            return await _Service1.GetAllCustomersAsync();
        }

        [HttpPost]
        [Route("PostNewEmployee")]
        public async Task<IActionResult> PostNewEmployeeAsync(PostNewEmployee postNewEmployee)
        {
            return await _Service1.PostNewEmployeeAsync(postNewEmployee);
        }

        [HttpPut]
        [Route("PutEmployee")]
        public async Task<IActionResult> PutEmployeeAsync(int id, PutEmployee putEmployee)
        {
            return await _Service1.PutEmployeeAsync(id, putEmployee);
        }

        [HttpDelete]
        [Route("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployeeAsync(int id)
        {
            return await _Service1.DeleteEmployeeAsync(id);
        }

        [HttpPut]
        [Route("PutCustomer")]
        public async Task<IActionResult> PutCustomerAsync(int id, PutCustomer putCustomer)
        {
            return await _Service1.PutCustomerAsync(id, putCustomer);
        }

        [HttpDelete]
        [Route("DeleteCustomer")]
        public async Task<IActionResult> DeleteCustomerAsync(int id)
        {
            return await _Service1.DeleteCustomerAsync(id);
        }

        [HttpPut]
        [Route("PutUserRole")]
        public async Task<IActionResult> PutUserRoleAsync(int id, PutUserRole putUserRole)
        {
            return await _Service1.PutUserRoleAsync(id, putUserRole);
        }
    }
}
